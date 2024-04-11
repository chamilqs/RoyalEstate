using AutoMapper;
using Microsoft.AspNetCore.Http;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.Property;
using RoyalState.Core.Application.ViewModels.PropertyImage;
using RoyalState.Core.Application.ViewModels.PropertyImprovement;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Services
{
    public class PropertyService : GenericService<SavePropertyViewModel, PropertyViewModel, Property>, IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IImprovementService _improvementService;
        private readonly IPropertyImageService _propertyImageService;
        private readonly IPropertyImprovementService _propertyImprovementService;
        private readonly IAgentService _agentService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse user;
        private readonly IMapper _mapper;

        public PropertyService(IPropertyRepository propertyRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper, IImprovementService improvementService, IPropertyImageService propertyImageService, IPropertyImprovementService propertyImprovementService, IAgentService agentService) : base(propertyRepository, mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _propertyRepository = propertyRepository;
            _mapper = mapper;
            _improvementService = improvementService;
            user = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _propertyImageService = propertyImageService;
            _propertyImprovementService = propertyImprovementService;
            _agentService = agentService;
        }

        #region Add Overriden
        public override async Task<SavePropertyViewModel> Add(SavePropertyViewModel vm)
        {
            vm.Code = await GenerateCode();
            var saveProperty = await base.Add(vm);

            // search for the property by code to get the id
            var findProperty = await GetPropertyByCode(vm.Code);

            foreach (var improvementId in vm.Improvements)
            {
                var improvement = await _improvementService.GetByIdViewModel(improvementId);
                SavePropertyImprovementViewModel propertyImprovement = new SavePropertyImprovementViewModel
                {
                    PropertyId = findProperty.Id,
                    ImprovementId = improvement.Id
                };

                await _propertyImprovementService.Add(propertyImprovement);

            }

            foreach (var image in vm.PropertyImages)
            {
                var propertyImage = new SavePropertyImageViewModel
                {
                    PropertyId = findProperty.Id,
                    ImageUrl = image
                };

                await _propertyImageService.Add(propertyImage);
            }

            return saveProperty;

        }
        #endregion

        #region Get Methods
        // configure it to get all properties with their images and improvements
        public async override Task<List<PropertyViewModel>> GetAllViewModel()
        {
            var properties = await base.GetAllViewModel();
            var propertiesViewModel = new List<PropertyViewModel>();

            foreach (var property in properties)
            {                
                var propertyImages = await _propertyImageService.GetImagesByPropertyId(property.Id);
                var propertyImprovements = await _propertyImprovementService.GetImprovementsByPropertyId(property.Id);
                var agent = await _agentService.GetByIdViewModel(property.AgentId);

                PropertyViewModel propertyViewModel = new PropertyViewModel
                {
                    Id = property.Id,
                    Code = property.Code,
                    PropertyTypeId = property.PropertyTypeId,
                    SaleTypeId = property.SaleTypeId,
                    Price = property.Price,
                    Meters = property.Meters,
                    Description = property.Description,
                    Bedrooms = property.Bedrooms,
                    Bathrooms = property.Bathrooms,
                    PropertyImages = propertyImages,
                    Improvements = propertyImprovements,

                    // Agent details
                    AgentId = property.AgentId,
                    AgentFirstName = agent.FirstName,
                    AgentLastName = agent.LastName,
                    AgentPhone = agent.Phone,
                    AgentEmail = agent.Email,
                    AgentImage = agent.ImageUrl
                };

                propertiesViewModel.Add(propertyViewModel);

            }

            return propertiesViewModel;
        }

        public async Task<PropertyViewModel> GetPropertyByCode(string code)
        {
            var propertiesList = await GetAllViewModel();

            return propertiesList.FirstOrDefault(sa => sa.Code == code);

        }

        public Task<PropertyViewModel> GetAllViewModelWIthFilters(FilterPropertyViewModel filterProperty)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods
        private async Task<string> GenerateCode()
        {
            string code = "000000";

            while (true)
            {
                Random randomNumber = new Random();
                int number = randomNumber.Next(100000, 1000000);
                code = number.ToString("D6");

                if (!await CodeExists(code))
                {
                    break;
                }
            }

            return code;
        }

        private async Task<bool> CodeExists(string code)
        {
            var propertiesList = await GetAllViewModel();

            if (propertiesList.Any(sa => sa.Code == code))
            {
                return true;
            }

            return false;
        }

        #endregion


    }
}
