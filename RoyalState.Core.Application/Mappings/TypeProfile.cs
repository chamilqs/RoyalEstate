using AutoMapper;
using RoyalState.Core.Application.ViewModels.PropertyTypes;
using RoyalState.Core.Application.ViewModels.SaleTypes;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Mappings
{
    public class TypeProfile : Profile
    {
        public TypeProfile()
        {
            #region SaleType
            CreateMap<SaleType, SaleTypeViewModel>()
                .ForMember(dest => dest.PropertiesQuantity, option => option.Ignore())
                .ReverseMap();

            CreateMap<SaleType, SaveSaleTypeViewModel>()
                .ReverseMap()
                .ForMember(origin => origin.Properties, option => option.Ignore());
            #endregion

            #region PropertyType
            CreateMap<PropertyType, PropertyTypeViewModel>()
                .ForMember(dest => dest.PropertiesQuantity, option => option.Ignore())
                .ReverseMap();

            CreateMap<PropertyType, SavePropertyTypeViewModel>()
                .ReverseMap()
                .ForMember(origin => origin.Properties, option => option.Ignore());
            #endregion
        }
    }
}
