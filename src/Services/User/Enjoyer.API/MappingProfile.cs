using AutoMapper;
using Enjoyer.Core.DataTransferObjects;
using Enjoyer.Core.Models;

namespace Enjoyer.API
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
