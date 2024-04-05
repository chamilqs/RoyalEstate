using RoyalState.Core.Application.ViewModels.Improvements;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Interfaces.Services
{
    public interface IImprovementService : IGenericService<SaveImprovementViewModel, ImprovementViewModel, SaleType>
    {
    }
}
