using AutoMapper;
using RoyalState.Core.Application.ViewModels.Improvements;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Mappings
{
    public class ImprovementProfile : Profile
    {
        public ImprovementProfile()
        {
            #region Improvement
            CreateMap<Improvement, ImprovementViewModel>()
                .ReverseMap();

            CreateMap<Improvement, SaveImprovementViewModel>()
                .ReverseMap();
            #endregion
        }
    }
}
