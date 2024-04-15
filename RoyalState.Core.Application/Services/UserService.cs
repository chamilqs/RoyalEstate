using AutoMapper;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Enums;
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
        /// <summary>
        /// Authenticates a user asynchronously.
        /// </summary>
        /// <param name="vm">The login view model.</param>
        /// <returns>The authentication response.</returns>
        public async Task<AuthenticationResponse> LoginAsync(LoginViewModel vm)
        {
            AuthenticationRequest loginRequest = _mapper.Map<AuthenticationRequest>(vm);
            AuthenticationResponse userResponse = await _accountService.AuthenticateWebAppAsync(loginRequest);

            return userResponse;
        }

        public async Task SignOutAsync()
        {
            await _accountService.SingOutAsync();
        }

        #endregion

        #region Email Confirmation
        /// <summary>
        /// Confirms the email of a user asynchronously.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="token">The confirmation token.</param>
        /// <returns>The confirmation result.</returns>
        public async Task<string> ConfirmEmailAsync(string userId, string token)
        {
            return await _accountService.ConfirmAccountAsync(userId, token);
        }

        #endregion

        #region Register
        /// <summary>
        /// Registers a user asynchronously.
        /// </summary>
        /// <param name="vm">The view model containing user information.</param>
        /// <param name="origin">The origin of the registration request.</param>
        /// <returns>The registration response.</returns>
        public async Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin)
        {
            RegisterRequest registerRequest = _mapper.Map<RegisterRequest>(vm);

            registerRequest.Role = vm.Role;

            return await _accountService.RegisterUserAsync(registerRequest, origin);
        }
        #endregion

        #region Get Methods

        #region GetByEmail
        /// <summary>
        /// Retrieves a user by email asynchronously.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <returns>The user view model.</returns>
        public async Task<UserViewModel> GetByEmailAsync(string email)
        {
            UserDTO userDTO = await _accountService.FindByEmailAsync(email);

            UserViewModel vm = _mapper.Map<UserViewModel>(userDTO);

            return vm;
        }
        #endregion

        #region GetById
        /// <summary>
        /// Retrieves a user by ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>The user view model.</returns>
        public async Task<UserViewModel> GetByIdAsync(string id)
        {
            UserDTO userDTO = await _accountService.FindByIdAsync(id);

            UserViewModel vm = _mapper.Map<UserViewModel>(userDTO);

            return vm;

        }
        #endregion

        #region GetByUserName
        /// <summary>
        /// Retrieves a list of users by name asynchronously.
        /// </summary>
        /// <param name="name">The name of the users.</param>
        /// <returns>The list of user view models.</returns>
        public async Task<List<UserViewModel>> GetByNameAsync(string name)
        {
            List<UserDTO> userDTO = await _accountService.FindByNameAsync(name);

            List<UserViewModel> vm = _mapper.Map<List<UserViewModel>>(userDTO);

            return vm;
        }
        #endregion

        #region GetUserSaveViewModel
        /// <summary>
        /// Retrieves a save user view model by user ID asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The save user view model.</returns>
        public async Task<SaveUserViewModel> GetUserSaveViewModel(string userId)
        {
            UserDTO userDTO = await _accountService.FindByIdAsync(userId);

            SaveUserViewModel userVm = new SaveUserViewModel()
            {
                Id = userDTO.Id,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                UserName = userDTO.UserName,
                Email = userDTO.Email,
                Phone = userDTO.Phone,
                Role = userDTO.Role == Roles.Admin.ToString() ? (int)Roles.Admin : (int)Roles.Client,
            };

            return userVm;

        }
        #endregion

        #endregion

        #region Update
        /// <summary>
        /// Updates a user asynchronously.
        /// </summary>
        /// <param name="vm">The view model containing user information.</param>
        /// <returns>The update response.</returns>
        public async Task<UpdateUserResponse> UpdateUserAsync(SaveUserViewModel vm)
        {
            UpdateUserRequest updateRequest = _mapper.Map<UpdateUserRequest>(vm);

            return await _accountService.UpdateUserAsync(updateRequest);
        }
        #endregion

        #region Delete
        public async Task<GenericResponse> DeleteUserAsync(string userId)
        {
            var response = await _accountService.DeleteUserAsync(userId);
            return response;

        }
        #endregion

    }
}
