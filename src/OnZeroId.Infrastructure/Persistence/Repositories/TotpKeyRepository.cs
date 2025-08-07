using OnZeroId.Domain.Entities;
using OnZeroId.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using OnZeroId.Infrastructure.Persistence.DbContexts;

namespace OnZeroId.Infrastructure.Persistence.Repositories
{
    using AutoMapper;

    public class TotpKeyRepository : ITotpKeyRepository
    {
        private readonly OnZeroIdDbContext _dbContext;
        private readonly IMapper _mapper;
        public TotpKeyRepository(OnZeroIdDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task AddAsync(TotpKey key, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<Entities.TotpKeys>(key);
            await _dbContext.TotpKeys.AddAsync(entity, cancellationToken).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        public async Task<TotpKey?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var entity = await _dbContext.TotpKeys.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken).ConfigureAwait(false);
            if (entity == null) return null;
            return _mapper.Map<TotpKey>(entity);
        }
        public async Task SetValidAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var entity = await _dbContext.TotpKeys.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken).ConfigureAwait(false);
            if (entity != null)
            {
                entity.IsActive = true;
                entity.LastUsedAt = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        public async Task DeleteAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var entity = await _dbContext.TotpKeys.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken).ConfigureAwait(false);
            if (entity != null)
            {
                _dbContext.TotpKeys.Remove(entity);
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        public async Task UpdateAsync(TotpKey key, CancellationToken cancellationToken = default)
        {
            var entity = await _dbContext.TotpKeys.FirstOrDefaultAsync(x => x.UserId == key.UserId, cancellationToken).ConfigureAwait(false);
            if (entity != null)
            {
                entity.Secret = key.Secret;
                entity.IsActive = key.IsValid;
                entity.UpdatedAt = key.UpdatedAt;
                // 其他欄位如有需要可一併更新
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
