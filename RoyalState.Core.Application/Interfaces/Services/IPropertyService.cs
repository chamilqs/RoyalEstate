using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.ViewModels.Agent;
using RoyalState.Core.Application.ViewModels.Property;

namespace RoyalState.Core.Application.Interfaces.Services
{
    public interface IPropertyService : IGenericService<SavePropertyViewModel, PropertyViewModel, Property>
    {
        Task<List<PropertyViewModel>> GetAllViewModelWIthFilters(FilterPropertyViewModel filterProperty);
        Task<List<PropertyViewModel>> GetAgentProperties(int id);
        Task<List<int>> GetPropertyQuantities(List<int> agentIds);
        Task<List<AgentViewModel>> GetAgentsWithPropertyQuantity();
        Task<GenericResponse> DeleteByAgentId(int id);
        Task<PropertyViewModel> GetPropertyByCode(string code);
    }
}
