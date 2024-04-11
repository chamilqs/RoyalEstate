using AutoMapper;
using RoyalState.Core.Application.ViewModels.PropertyImprovement;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Mappings
{
    public class PropertyImprovementProfile : Profile
    {
        public PropertyImprovementProfile()
        {
            #region PropertyImprovementProfile
            CreateMap<PropertyImprovement, SavePropertyImprovementViewModel>()
                .ReverseMap()
                .ForMember(x => x.Improvement, opt => opt.Ignore())
                .ForMember(x => x.Property, opt => opt.Ignore());

            CreateMap<PropertyImprovement, PropertyImprovementViewModel>()
                .ReverseMap()
                .ForMember(x => x.Improvement, opt => opt.Ignore())
                .ForMember(x => x.Property, opt => opt.Ignore());
            #endregion
        }
    }
}
