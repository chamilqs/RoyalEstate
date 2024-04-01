using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.ViewModels.Users;

namespace RoyalState.Core.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<AuthenticationResponse> LoginAsync(LoginViewModel vm);
        //Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin);
        Task SignOutAsync();
    }
}