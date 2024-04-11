using RoyalState.Core.Application.ViewModels.PropertyImprovement;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Interfaces.Services
{
    public interface IPropertyImprovementService : IGenericService<SavePropertyImprovementViewModel, PropertyImprovementViewModel, PropertyImprovement>
    {
        Task<List<string>> GetImprovementsByPropertyId(int propertyId);
    }
}
