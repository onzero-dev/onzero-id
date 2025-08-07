using AutoMapper;
using OnZeroId.Domain.Entities;
using OnZeroId.Infrastructure.Persistence.Entities;

namespace OnZeroId.Infrastructure.Persistence.Mappings;

public class InfraTotpKeyToDomainTotpKeyProfile : Profile
{
    public InfraTotpKeyToDomainTotpKeyProfile()
    {
        CreateMap<TotpKeys, TotpKey>()
            .ForMember(dest => dest.IsValid, opt => opt.MapFrom(src => src.IsActive));
        CreateMap<TotpKey, TotpKeys>()
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsValid));
    }
}
