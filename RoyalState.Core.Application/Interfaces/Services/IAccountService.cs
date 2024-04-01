using RoyalState.Core.Application.DTOs.Account;

namespace RoyalState.Core.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task SingOutAsync();
    }
}