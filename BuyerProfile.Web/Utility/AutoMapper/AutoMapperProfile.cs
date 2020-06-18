using AutoMapper;
using BuyerProfile.Web.Models;
using DAL.Models;
using BuyerProfile.Utilitis;

namespace BuyerProfile.Utilitis.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Tb_Sell, SellDto>()
            .ReverseMap();

            CreateMap<ApplicationUser, ProfileDto>()
            .ReverseMap();
        }
    }
}
