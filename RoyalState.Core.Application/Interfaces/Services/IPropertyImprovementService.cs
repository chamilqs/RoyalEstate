using RoyalState.Core.Application.ViewModels.PropertyImprovement;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Interfaces.Services
{
    public interface IPropertyImprovementService : IGenericService<SavePropertyImprovementViewModel, PropertyImprovementViewModel, PropertyImprovement>
    {
        Task<List<string>> GetImprovementsNamesByPropertyId(int propertyId);
        Task DeleteImprovementsByPropertyId(int propertyId);
        Task<List<PropertyImprovementViewModel>> GetImprovementsByPropertyId(int propertyId);
    }
}
