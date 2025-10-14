using Domain.Identity;
using Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Application.Common.Interfaces;

public interface IKonsentalkDbContext
{
    DbSet<UserAggregate> Users { get; set; }
    DbSet<IdentityAggregate> Identities { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    DatabaseFacade Database { get; }
}