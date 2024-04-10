using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.ViewModels.Agent;
using RoyalState.Core.Application.ViewModels.Users;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Interfaces.Services
{
    public interface IAgentService : IGenericService<SaveAgentViewModel, AgentViewModel, Agent>
    {
        Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin);
        Task<UpdateUserResponse> UpdateAsync(SaveUserViewModel vm);
        Task<AgentViewModel> GetByUserIdViewModel(string userId);
        Task<List<AgentViewModel>> GetByNameViewModel(string name);
        Task<SaveUserViewModel> GetProfileDetails();

    }
}
