using OnZeroId.Domain.Entities;
using OnZeroId.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using OnZeroId.Infrastructure.Persistence.DbContexts;

namespace OnZeroId.Infrastructure.Persistence.Repositories;


using AutoMapper;

public class UserRepository : IUserRepository
{
    private readonly OnZeroIdDbContext _dbContext;
    private readonly IMapper _mapper;

    public UserRepository(OnZeroIdDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }


    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<Entities.Users>(user);
        await _dbContext.Users.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken)
            .ConfigureAwait(false);
        if (entity == null) return null;
        return _mapper.Map<User>(entity);
    }
}
