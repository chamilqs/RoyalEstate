using AutoMapper;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.Client;
using RoyalState.Core.Application.ViewModels.Users;
using RoyalState.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using RoyalState.Core.Application.ViewModels.ClientProperties;
using RoyalState.Core.Application.ViewModels.Property;

namespace RoyalState.Core.Application.Services
{
    #region ClientService
    public class ClientService : GenericService<SaveClientViewModel, ClientViewModel, Client>, IClientService
    {
        #region Fields
        private readonly IClientRepository _clientRepository;
        private readonly IClientPropertiesService _clientPropertiesService;
        private readonly IPropertyService _propertiesService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse user;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public ClientService(IClientRepository clientRepository, IClientPropertiesService clientPropertiesService, IUserService userService, IHttpContextAccessor httpContextAccessor, IMapper mapper, IPropertyService propertiesService) : base(clientRepository, mapper)
        {
            _clientRepository = clientRepository;
            _clientPropertiesService = clientPropertiesService;
            _userService = userService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            user = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _propertiesService = propertiesService;
        }
        #endregion

        #region GetByUserIdViewModel
        public async Task<ClientViewModel> GetByUserIdViewModel(string userId)
        {
            var clientList = await GetAllViewModel();
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

        #region MarkPropertyAsFavorite
        public async Task<SaveClientPropertiesViewModel> MarkPropertyAsFavorite(SaveClientPropertiesViewModel vm)
        {
            await _clientPropertiesService.Add(vm);

            return vm;
        }

        #endregion

        #region DeleteFavorite
        public async Task DeleteFavorite(int id)
        {
            var clientProperties = await _clientPropertiesService.GetByPropertyIdViewModel(id);

            await _clientPropertiesService.Delete(clientProperties.Id);
        }
        #endregion

        #region GetFavoriteProperties
        public async Task<List<int>> GetIdsOfFavoriteProperties(int id)
        {
            var clientProperties = await _clientPropertiesService.GetAllViewModel();


            List<int> properties = new List<int>();
            foreach (var property in clientProperties)
            {
                if (property.ClientId == id)
                {
                    properties.Add(property.PropertyId);
                }

            }

            return properties;
        }
        #endregion

        #region GetFavoritePropertiesViewModel
        public async Task<List<PropertyViewModel>> GetFavoritePropertiesViewModel(int id)
        {
            var properties = await _propertiesService.GetAllViewModel();
            var clientProperties = await _clientPropertiesService.GetAllViewModel();

            var favoritePropertyIds = clientProperties
                .Where(client => client.ClientId == id)
                .Select(client => client.PropertyId);

            List<PropertyViewModel> favoriteProperties = properties
                .Where(property => favoritePropertyIds.Contains(property.Id))
                .ToList();

            return favoriteProperties;
        }


        #endregion


    }
    #endregion
}
