using Application.Common.Interfaces;
using Domain.User;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserRepositories(IKonsentalkDbContext context): IUserRepository
{
    public async Task AddAsync(UserAggregate user, CancellationToken token)
    {
        await context.Users.AddAsync(user, token);
        await context.SaveChangesAsync(token);
    }
    public async Task<bool> IsExistAsync(string? email, CancellationToken token)
    {
        return await context.Users.AnyAsync(x => x.Email == email, token);
    }
    public async Task UpdateAsync(UserAggregate user, CancellationToken token)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync(token);
    }
}