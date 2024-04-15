using AutoMapper;
using RoyalState.Core.Application.ViewModels.ClientProperties;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Mappings
{
    public class ClientPropertiesProfile : Profile
    {
        public ClientPropertiesProfile()
        {
            #region ClientProperties
            CreateMap<ClientProperties, ClientPropertiesViewModel>()
                    .ReverseMap()
                    .ForMember(dest => dest.Property, opt => opt.Ignore())
                    .ForMember(dest => dest.Client, opt => opt.Ignore());

            CreateMap<ClientProperties, SaveClientPropertiesViewModel>()
                .ReverseMap()
                .ForMember(dest => dest.Property, opt => opt.Ignore())
                .ForMember(dest => dest.Client, opt => opt.Ignore());
            #endregion
        }
    }
}
