using AutoMapper;
using RoyalState.Core.Application.ViewModels.Property;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Mappings
{
    public class PropertyProfile : Profile
    {
        public PropertyProfile()
        {
            #region Property
            CreateMap<Property, SavePropertyViewModel>()
                .ForMember(x => x.PropertyImages, opt => opt.Ignore())
                .ForMember(x => x.Improvements, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedDate, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())

                .ForMember(x => x.PropertyType, opt => opt.Ignore())
                .ForMember(x => x.SaleType, opt => opt.Ignore())
                .ForMember(x => x.Agent, opt => opt.Ignore())
                .ForMember(x => x.PropertyImages, opt => opt.Ignore())
                .ForMember(x => x.Improvements, opt => opt.Ignore())
                .ForMember(x => x.PropertyImprovements, opt => opt.Ignore())
                .ForMember(x => x.Clients, opt => opt.Ignore())
                .ForMember(x => x.ClientProperties, opt => opt.Ignore());


            CreateMap<Property, PropertyViewModel>()
                .ForMember(x => x.AgentFirstName, opt => opt.Ignore())
                .ForMember(x => x.AgentLastName, opt => opt.Ignore())
                .ForMember(x => x.AgentPhone, opt => opt.Ignore())
                .ForMember(x => x.AgentEmail, opt => opt.Ignore())
                .ForMember(x => x.AgentImage, opt => opt.Ignore())

                .ForMember(x => x.PropertyImages, opt => opt.MapFrom(src => src.PropertyImages))
                .ForMember(x => x.Improvements, opt => opt.MapFrom(src => src.Improvements))
                .ReverseMap()
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedDate, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())

                .ForMember(x => x.PropertyType, opt => opt.Ignore())
                .ForMember(x => x.SaleType, opt => opt.Ignore())
                .ForMember(x => x.Agent, opt => opt.Ignore())
                .ForMember(x => x.PropertyImages, opt => opt.Ignore())
                .ForMember(x => x.Improvements, opt => opt.Ignore())
                .ForMember(x => x.PropertyImprovements, opt => opt.Ignore())
                .ForMember(x => x.Clients, opt => opt.Ignore())
                .ForMember(x => x.ClientProperties, opt => opt.Ignore());
            #endregion
        }
    }
}
