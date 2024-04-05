using AutoMapper;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.Client;
using RoyalState.Core.Application.ViewModels.Users;
using RoyalState.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace RoyalState.Core.Application.Services
{
    public class ClientService : GenericService<SaveClientViewModel, ClientViewModel, Client>, IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse user;
        private readonly IMapper _mapper;

        public ClientService(IClientRepository clientRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper, IUserService userService) : base(clientRepository, mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _clientRepository = clientRepository;
            _mapper = mapper;
            user = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _userService = userService;
        }

        #region GetByUserIdViewModel
        public async Task<ClientViewModel> GetByUserIdViewModel(string userId)
        {
            var clientList = await base.GetAllViewModel();
            ClientViewModel client = clientList.FirstOrDefault(client => client.UserId == userId);

            return client;
        }
        #endregion

        #region Register
        public async Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin)
        {
            RegisterResponse response = await _userService.RegisterAsync(vm, origin);

            if (!response.HasError)
            {
                var user = await _userService.GetByEmailAsync(vm.Email);

                SaveClientViewModel saveClientViewModel = new()
                {
                    UserId = user.Id,
                    ImageUrl = vm.ImageUrl,
                };

                await base.Add(saveClientViewModel);

            }

            return response;
        }
        #endregion

        
    }
}
