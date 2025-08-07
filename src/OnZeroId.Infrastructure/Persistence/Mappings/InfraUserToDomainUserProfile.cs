using AutoMapper;
using OnZeroId.Domain.Entities;
using OnZeroId.Infrastructure.Persistence.Entities;

namespace OnZeroId.Infrastructure.Persistence.Mappings;

public class InfraUserToDomainUserProfile : Profile
{
    public InfraUserToDomainUserProfile()
    {
        CreateMap<Users, User>();
        CreateMap<User, Users>();
    }
}
