using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.ViewModels.Client;
using RoyalState.Core.Application.ViewModels.Users;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Interfaces.Services
{
    public interface IClientService : IGenericService<SaveClientViewModel, ClientViewModel, Client>
    {
        Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin);

    }
}
