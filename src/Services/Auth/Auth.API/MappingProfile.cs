using AutoMapper;
using Auth.Core.DataTransferObjects;
using Auth.Core.Models;

namespace Auth.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserRegistrationModel, User>();
            CreateMap<ApplicationUserDto, ApplicationUser>();
            CreateMap<RoleForCreation, Role>();
            CreateMap<UserForCreation, User>();
        }
    }
}
