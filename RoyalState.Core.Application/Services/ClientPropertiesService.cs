using AutoMapper;
using Microsoft.AspNetCore.Http;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.ClientProperties;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Services
{
    public class ClientPropertiesService : GenericService<SaveClientPropertiesViewModel, ClientPropertiesViewModel, ClientProperties>, IClientPropertiesService
    {
        private readonly IClientPropertiesRepository _clientPropertiesRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse user;
        private readonly IMapper _mapper;

        public ClientPropertiesService(IClientPropertiesRepository clientPropertiesRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(clientPropertiesRepository, mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _clientPropertiesRepository = clientPropertiesRepository;
            _mapper = mapper;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            user = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        #region GetByPropertyIdViewModel
        /// <summary>
        /// Retrieves a <see cref="ClientPropertiesViewModel"/> by property ID.
        /// </summary>
        /// <param name="propertyId">The ID of the property.</param>
        /// <returns>The <see cref="ClientPropertiesViewModel"/> with the specified property ID, or null if not found.</returns>
        public async Task<ClientPropertiesViewModel> GetByPropertyIdViewModel(int propertyId)
        {
            var clientPropertiesList = await GetAllViewModel();
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            ClientPropertiesViewModel clientProperties = clientPropertiesList.FirstOrDefault(clientProperties => clientProperties.PropertyId == propertyId);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

#pragma warning disable CS8603 // Possible null reference return.
            return clientProperties;
#pragma warning restore CS8603 // Possible null reference return.
        }
        #endregion

    }
}
