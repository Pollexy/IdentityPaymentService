using Application.Common.Interfaces;
using Domain.Identity;
using Domain.User;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class KonsentalkDbContext:BaseDbContext, IKonsentalkDbContext
{
    public KonsentalkDbContext(DbContextOptions<KonsentalkDbContext> options) : base(options)
    {
    }
    
    public DbSet<IdentityAggregate> Identities { get; set; }
    public DbSet<UserAggregate> Users { get; set; }
        
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        base.OnConfiguring(optionsBuilder);
    }
}