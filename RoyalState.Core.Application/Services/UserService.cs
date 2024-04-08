using AutoMapper;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.User;
using RoyalState.Core.Application.ViewModels.Users;

namespace RoyalState.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public UserService(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        #region Login & Logout
        public async Task<AuthenticationResponse> LoginAsync(LoginViewModel vm)
        {
            AuthenticationRequest loginRequest = _mapper.Map<AuthenticationRequest>(vm);
            AuthenticationResponse userResponse = await _accountService.AuthenticateAsync(loginRequest);

            return userResponse;
        }

        public async Task SignOutAsync()
        {
            await _accountService.SingOutAsync();
        }

        #endregion

        #region Email Confirmation
        public async Task<string> ConfirmEmailAsync(string userId, string token)
        {
            return await _accountService.ConfirmAccountAsync(userId, token);
        }

        #endregion

        #region Register
        public async Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin)
        {
            RegisterRequest registerRequest = _mapper.Map<RegisterRequest>(vm);

            registerRequest.Role = vm.Role;

            return await _accountService.RegisterUserAsync(registerRequest, origin);
        }
        #endregion

        #region Get Methods
        public async Task<UserViewModel> GetByEmailAsync(string email)
        {
            UserDTO userDTO = await _accountService.FindByEmailAsync(email);

            UserViewModel vm = _mapper.Map<UserViewModel>(userDTO);

            return vm;
        }

        public async Task<UserViewModel> GetByIdAsync(string id)
        {
            UserDTO userDTO = await _accountService.FindByIdAsync(id);

            UserViewModel vm = _mapper.Map<UserViewModel>(userDTO);

            return vm;

        }

        public async Task<List<UserViewModel>> GetByNameAsync(string name)
        {
            List<UserDTO> userDTO = await _accountService.FindByNameAsync(name);

            List<UserViewModel> vm = _mapper.Map<List<UserViewModel>>(userDTO);

            return vm;
        }
        #endregion
    }
}
