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
    internal class ClientPropertiesService : GenericService<SaveClientPropertiesViewModel, ClientPropertiesViewModel, ClientProperties>, IClientPropertiesService
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
            user = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        #region GetByPropertyIdViewModel
        public async Task<ClientPropertiesViewModel> GetByPropertyIdViewModel(int propertyId)
        {
            var clientPropertiesList = await GetAllViewModel();
            ClientPropertiesViewModel clientProperties = clientPropertiesList.FirstOrDefault(clientProperties => clientProperties.PropertyId == propertyId);

            return clientProperties;
        }
        #endregion

    }
}
