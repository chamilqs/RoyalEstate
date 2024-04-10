using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.ViewModels.User;
using RoyalState.Core.Application.ViewModels.Users;

namespace RoyalState.Core.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<string> ConfirmEmailAsync(string userId, string token);
        Task<AuthenticationResponse> LoginAsync(LoginViewModel vm);
        Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin);
        Task<UpdateUserResponse> UpdateUserAsync(SaveUserViewModel vm);
        Task<SaveUserViewModel> GetUserSaveViewModel(string userId);
        Task<UserViewModel> GetByEmailAsync(string email);
        Task<List<UserViewModel>> GetByNameAsync(string name);
        Task<UserViewModel> GetByIdAsync(string id);
        Task SignOutAsync();
    }
}