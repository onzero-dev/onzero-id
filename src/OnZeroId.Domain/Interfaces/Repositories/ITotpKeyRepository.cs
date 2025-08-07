using OnZeroId.Domain.Entities;

namespace OnZeroId.Domain.Interfaces.Repositories;

public interface ITotpKeyRepository
{
    Task AddAsync(TotpKey key, CancellationToken cancellationToken = default);
    Task<TotpKey?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task SetValidAsync(Guid userId, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid userId, CancellationToken cancellationToken = default);
    Task UpdateAsync(TotpKey key, CancellationToken cancellationToken = default);
}
