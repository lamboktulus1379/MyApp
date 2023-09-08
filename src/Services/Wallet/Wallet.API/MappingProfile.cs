using AutoMapper;
using Wallet.Core.DataTransferObjects;
using Wallet.Core.Models;

namespace Wallet.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserBalanceForCreation, UserBalance>();
        }
    }
}
