using AutoMapper;
using Microsoft.AspNetCore.Http;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.Client;
using RoyalState.Core.Application.ViewModels.ClientProperties;
using RoyalState.Core.Application.ViewModels.Property;
using RoyalState.Core.Application.ViewModels.Users;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Services
{
    public class ClientService : GenericService<SaveClientViewModel, ClientViewModel, Client>, IClientService
    {

        private readonly IClientRepository _clientRepository;
        private readonly IClientPropertiesService _clientPropertiesService;
        private readonly IPropertyService _propertiesService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse user;
        private readonly IMapper _mapper;

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

        #region Register
        /// <summary>
        /// Registers a user asynchronously and adds a client view model if registration is successful.
        /// </summary>
        /// <param name="vm">The SaveUserViewModel containing user registration information.</param>
        /// <param name="origin">The origin of the registration request.</param>
        /// <returns>A RegisterResponse indicating the result of the registration.</returns>
        public async Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin)
        {
            RegisterResponse response = await _userService.RegisterAsync(vm, origin);

            if (!response.HasError)
            {
                var user = await _userService.GetByEmailAsync(vm.Email);

#pragma warning disable CS8601 // Possible null reference assignment.
                SaveClientViewModel saveClientViewModel = new()
                {
                    UserId = user.Id,
                    ImageUrl = vm.ImageUrl,
                };
#pragma warning restore CS8601 // Possible null reference assignment.

                await base.Add(saveClientViewModel);

            }

            return response;
        }
        #endregion

        #region MarkPropertyAsFavorite
        /// <summary>
        /// Marks a property as favorite for a client.
        /// </summary>
        /// <param name="vm">The SaveClientPropertiesViewModel containing the property information.</param>
        /// <returns>The SaveClientPropertiesViewModel.</returns>
        public async Task<SaveClientPropertiesViewModel> MarkPropertyAsFavorite(SaveClientPropertiesViewModel vm)
        {
            await _clientPropertiesService.Add(vm);

            return vm;
        }

        #endregion

        #region DeleteFavorite
        /// <summary>
        /// Deletes a favorite property for a client.
        /// </summary>
        /// <param name="id">The ID of the property to delete.</param>
        public async Task DeleteFavorite(int id)
        {
            var clientProperties = await _clientPropertiesService.GetByPropertyIdViewModel(id);

            await _clientPropertiesService.Delete(clientProperties.Id);
        }
        #endregion

        #region Get Methods

        #region GetByUserIdViewModel
        /// <summary>
        /// Retrieves a client view model by user ID.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>The client view model.</returns>
        public async Task<ClientViewModel> GetByUserIdViewModel(string userId)
        {
            var clientList = await GetAllViewModel();
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            ClientViewModel client = clientList.FirstOrDefault(client => client.UserId == userId);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

#pragma warning disable CS8603 // Possible null reference return.
            return client;
#pragma warning restore CS8603 // Possible null reference return.
        }
        #endregion

        #region GetFavoriteProperties
        /// <summary>
        /// Retrieves the IDs of favorite properties for a client.
        /// </summary>
        /// <param name="id">The ID of the client.</param>
        /// <returns>A list of property IDs.</returns>
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
        /// <summary>
        /// Retrieves the favorite properties for a client.
        /// </summary>
        /// <param name="id">The ID of the client.</param>
        /// <returns>A list of favorite properties.</returns>
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

        #endregion
    }
}
