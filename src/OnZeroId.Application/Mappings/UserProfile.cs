using AutoMapper;
using OnZeroId.Domain.Entities;
using OnZeroId.Application.DTOs;

namespace OnZeroId.Application.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
    }
}
