using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RoyalState.Core.Application.ViewModels.Property;

namespace RoyalState.Core.Application.Interfaces.Services
{
    public interface IPropertyService : IGenericService<SavePropertyViewModel, PropertyViewModel, Property>
    {
        Task<PropertyViewModel> GetAllViewModelWIthFilters(FilterPropertyViewModel filterProperty);
        Task<List<PropertyViewModel>> GetAgentProperties(int id);
        Task<PropertyViewModel> GetPropertyByCode(string code);
    }
}
