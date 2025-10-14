using Domain.User;

namespace Application.Common.Interfaces;

public interface IUserRepository
{
    Task<bool> IsExistAsync(string? email, CancellationToken token);
    Task AddAsync(UserAggregate user, CancellationToken token);
    Task UpdateAsync(UserAggregate user, CancellationToken token);
}