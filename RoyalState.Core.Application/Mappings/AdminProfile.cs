using AutoMapper;
using RoyalState.Core.Application.ViewModels.Admins;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Mappings
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            #region Admin
            CreateMap<Admin, AdminViewModel>()
                .ForMember(dest => dest.Username, option => option.Ignore())
                .ForMember(dest => dest.FirstName, option => option.Ignore())
                .ForMember(dest => dest.LastName, option => option.Ignore())
                .ForMember(dest => dest.Email, option => option.Ignore())
                .ForMember(dest => dest.EmailConfirmed, option => option.Ignore())
                .ReverseMap();

            CreateMap<Admin, SaveAdminViewModel>()
                .ReverseMap()
                .ForMember(origin => origin.LastModifiedBy, option => option.Ignore())
                .ForMember(origin => origin.LastModifiedDate, option => option.Ignore());
            #endregion
        }
    }
}
