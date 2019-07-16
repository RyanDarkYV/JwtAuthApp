using AutoMapper;
using JwtAuthApp.Server.Models.Identity;

namespace JwtAuthApp.Server.ViewModels.Mappings
{
    public class ViewModelToEntityMappingProfile : Profile
    {
        public ViewModelToEntityMappingProfile()
        {
            CreateMap<RegistrationViewModel, AppUser>().ForMember(x => x.UserName, map => map.MapFrom(x => x.Email));
        }
    }
}