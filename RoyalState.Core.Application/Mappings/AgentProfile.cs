using AutoMapper;
using RoyalState.Core.Application.ViewModels.Agent;
using RoyalState.Core.Application.ViewModels.User;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Mappings
{
    public class AgentProfile : Profile
    {
        public AgentProfile()
        {
            #region AgentProfile
            CreateMap<Agent, SaveAgentViewModel>()
                .ReverseMap()
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedDate, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.Properties, opt => opt.Ignore());

            CreateMap<Agent, AgentViewModel>()
                .ReverseMap()
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedDate, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.Properties, opt => opt.Ignore());

            #endregion
        }
    }
}
