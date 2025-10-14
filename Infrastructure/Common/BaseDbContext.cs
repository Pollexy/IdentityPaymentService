using System.Linq.Expressions;
using Domain.Common.BaseModels;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common;

public interface ICurrentUserAccessor
{
    Guid? GetCurrentUserId();
}

public abstract class BaseDbContext : DbContext
{
    private readonly ICurrentUserAccessor? _currentUser;

    protected BaseDbContext(
        DbContextOptions options,
        ICurrentUserAccessor? currentUser = null) : base(options)
    {
        _currentUser = currentUser;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        EnsureAggregateModifications();
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        ApplyAggregateProperties(modelBuilder);
        ApplyGlobalSoftDeleteFilter(modelBuilder);
    }

    private void EnsureAggregateModifications()
    {
        var entries = ChangeTracker.Entries<BaseAggregate>()
            .Where(e => e.State is not EntityState.Unchanged)
            .ToList();

        var currentUserId = _currentUser?.GetCurrentUserId();

        foreach (var e in entries)
        {
            var aggregate = e.Entity;

            if (e.State == EntityState.Added)
            {
                aggregate.SetAsCreated();
            }
            else if (e.State == EntityState.Modified || e.State == EntityState.Deleted)
            {
                aggregate.SetAsModified(currentUserId);
            }
        }
    }

    private void ApplyAggregateProperties(ModelBuilder modelBuilder)
    {
        var types = modelBuilder.Model.GetEntityTypes()
            .Where(t => typeof(BaseAggregate).IsAssignableFrom(t.ClrType));

        foreach (var t in types)
        {
            var b = modelBuilder.Entity(t.ClrType);
            b.Ignore("IsModified");

            b.Property("Id").HasColumnName("id");
            b.Property("CreatedDate").HasColumnName("created_date").HasColumnType("timestamptz");
            b.Property("LastModifiedDate").HasColumnName("last_modified_date").HasColumnType("timestamptz");
            b.Property("IsDeleted").HasColumnName("is_deleted");
            b.Property("CreatedByUserId").HasColumnName("created_by_user_id");
            b.Property("UpdatedByUserId").HasColumnName("updated_by_user_id");

            b.HasKey("Id");
            b.HasIndex("CreatedDate");
            b.HasIndex("LastModifiedDate");
            b.HasIndex("CreatedByUserId");
            b.HasIndex("UpdatedByUserId");
        }
    }

    private void ApplyGlobalSoftDeleteFilter(ModelBuilder modelBuilder)
    {
        var types = modelBuilder.Model.GetEntityTypes()
            .Where(t => typeof(BaseAggregate).IsAssignableFrom(t.ClrType));

        foreach (var t in types)
        {
            var parameter = Expression.Parameter(t.ClrType, "e");
            var prop = Expression.Property(parameter, "IsDeleted");
            var body = Expression.Equal(prop, Expression.Constant(false));
            var lambda = Expression.Lambda(body, parameter);

            modelBuilder.Entity(t.ClrType).HasQueryFilter(lambda);
        }
    }
}