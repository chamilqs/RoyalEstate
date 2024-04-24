using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.ViewModels.Client;
using RoyalState.Core.Application.ViewModels.ClientProperties;
using RoyalState.Core.Application.ViewModels.Property;
using RoyalState.Core.Application.ViewModels.Users;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Interfaces.Services
{
    public interface IClientService : IGenericService<SaveClientViewModel, ClientViewModel, Client>
    {
        Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin);
        Task DeleteFavorite(int id);
        Task<ClientViewModel> GetByUserIdViewModel(string userId);
        Task<List<PropertyViewModel>> GetFavoritePropertiesViewModel(int id);
        Task<List<int>> GetIdsOfFavoriteProperties(int id);
        Task<SaveClientPropertiesViewModel> MarkPropertyAsFavorite(SaveClientPropertiesViewModel vm);

    }
}
