using RoyalState.Core.Application.DTOs.Account;
namespace RoyalState.Core.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<string> ConfirmAccountAsync(string userId, string token);
        Task<AuthenticationResponse> AuthenticateWebApiAsync(AuthenticationRequest request);
        Task<AuthenticationResponse> AuthenticateWebAppAsync(AuthenticationRequest request);
        Task<RegisterResponse> RegisterUserAsync(RegisterRequest request, string origin);
        Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest request);
        Task<GenericResponse> UpdateUserStatusAsync(string userId);
        Task<UserDTO> FindByEmailAsync(string email);
        Task<UserDTO> FindByIdAsync(string id);
        Task<List<UserDTO>> FindByNameAsync(string name);
        Task SingOutAsync();
        Task<List<UserDTO>> GetAllAgentAsync();
        Task<List<UserDTO>> GetAllAdminAsync();
        Task<List<UserDTO>> GetAllDevloperAsync();
        Task<List<UserDTO>> GetAllClientAsync();
    }
}