using AutoMapper;
using RoyalState.Core.Application.ViewModels.Client;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Mappings
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            #region Client
            CreateMap<Client, SaveClientViewModel>()
                .ReverseMap()
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedDate, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.FavoriteProperties, opt => opt.Ignore())
                .ForMember(x => x.ClientProperties, opt => opt.Ignore());

            CreateMap<Client, ClientViewModel>()
                .ReverseMap()
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedDate, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.FavoriteProperties, opt => opt.Ignore())
                .ForMember(x => x.ClientProperties, opt => opt.Ignore());
            #endregion
        }
    }
}
