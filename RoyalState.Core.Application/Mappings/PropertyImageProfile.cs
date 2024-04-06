
using AutoMapper;
using RoyalState.Core.Application.ViewModels.PropertyImage;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Mappings
{
    public class PropertyImageProfile : Profile
    {
        public PropertyImageProfile()
        {
            #region PropertyImage
            CreateMap<PropertyImage, SavePropertyImageViewModel>()
                .ReverseMap()
                .ForMember(dest => dest.Property, opt => opt.Ignore());

            CreateMap<PropertyImage, PropertyImageViewModel>()
                .ReverseMap()
                .ForMember(dest => dest.Property, opt => opt.Ignore());
            #endregion
        }
    }
}
