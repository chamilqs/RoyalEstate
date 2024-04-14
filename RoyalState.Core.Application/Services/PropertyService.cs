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
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly ISaleTypeService _saleTypeService;
        private readonly IPropertyImprovementService _propertyImprovementService;
        private readonly IAgentService _agentService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse user;
        private readonly IMapper _mapper;

        public PropertyService(IPropertyRepository propertyRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper, IImprovementService improvementService, IPropertyImageService propertyImageService, IPropertyImprovementService propertyImprovementService, IAgentService agentService, IPropertyTypeService propertyTypeService, ISaleTypeService saleTypeService) : base(propertyRepository, mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _propertyRepository = propertyRepository;
            _mapper = mapper;
            _improvementService = improvementService;
            user = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _propertyImageService = propertyImageService;
            _propertyImprovementService = propertyImprovementService;
            _agentService = agentService;
            _propertyTypeService = propertyTypeService;
            _saleTypeService = saleTypeService;
        }

        #region Add Overriden
        /// <summary>
        /// Adds a new property with the specified view model.
        /// </summary>
        /// <param name="vm">The view model containing the property data.</param>
        /// <returns>The view model of the added property.</returns>
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

        #region Update Overriden
        /// <summary>
        /// Updates a property with the specified view model and ID.
        /// </summary>
        /// <param name="vm">The view model containing the updated property data.</param>
        /// <param name="id">The ID of the property to update.</param>
        /// <returns>The updated view model of the property.</returns>
        public override async Task<SavePropertyViewModel> Update(SavePropertyViewModel vm, int id)
        {
            var property = await GetByIdViewModel(id);

            if (property == null)
            {
                return null;
            }

            Property propertyUpdate = new Property
            {
                Id = id,
                Code = vm.Code,
                PropertyTypeId = vm.PropertyTypeId,
                SaleTypeId = vm.SaleTypeId,
                Price = vm.Price,
                Meters = vm.Meters,
                Description = vm.Description,
                Bedrooms = vm.Bedrooms,
                Bathrooms = vm.Bathrooms,
                AgentId = (int)vm.AgentId,
                CreatedBy = user.UserName,
                CreatedDate = DateTime.UtcNow,
                LastModifiedBy = user.UserName,
                LastModifiedDate = DateTime.UtcNow
            };

            await _propertyRepository.UpdateAsync(propertyUpdate, id);

            // Delete all improvements and images associated with the property
            await _propertyImprovementService.DeleteImprovementsByPropertyId(id);
            await _propertyImageService.DeleteImagesByPropertyId(id);

            foreach (var improvementId in vm.Improvements)
            {
                var improvement = await _improvementService.GetByIdViewModel(improvementId);
                SavePropertyImprovementViewModel propertyImprovement = new SavePropertyImprovementViewModel
                {
                    PropertyId = vm.Id,
                    ImprovementId = improvement.Id
                };

                await _propertyImprovementService.Add(propertyImprovement);

            }

            foreach (var image in vm.PropertyImages)
            {
                var propertyImage = new SavePropertyImageViewModel
                {
                    PropertyId = id,
                    ImageUrl = image
                };

                await _propertyImageService.Add(propertyImage);
            }

            return vm;
        }
        #endregion

        #region Delete Overriden
        /// <summary>
        /// Deletes a property by its ID, along with its associated improvements and images.
        /// </summary>
        /// <param name="id">The ID of the property to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public override async Task Delete(int id)
        {
            await _propertyImprovementService.DeleteImprovementsByPropertyId(id);
            await _propertyImageService.DeleteImagesByPropertyId(id);
            await base.Delete(id);
        }
        #endregion

        #region Get Methods

        #region GetAllViewModel Overriden
        /// <summary>
        /// Retrieves all property view models.
        /// </summary>
        /// <returns>A list of property view models.</returns>
        /// Unneficient way to get the properties, but it is the way I decided for now.
        public async override Task<List<PropertyViewModel>> GetAllViewModel()
        {
            var properties = await _propertyRepository.GetAllAsync();
            properties = properties.OrderByDescending(p => p.CreatedDate).ToList();

            var propertiesViewModel = new List<PropertyViewModel>();

            foreach (var property in properties)
            {
                var propertyImages = await _propertyImageService.GetImagesUrlByPropertyId(property.Id);
                var propertyImprovements = await _propertyImprovementService.GetImprovementsNamesByPropertyId(property.Id);
                var agent = await _agentService.GetByIdViewModel(property.AgentId);
                var propertyType = await _propertyTypeService.GetByIdViewModel(property.PropertyTypeId);
                var saleType = await _saleTypeService.GetByIdViewModel(property.SaleTypeId);

                PropertyViewModel propertyViewModel = new PropertyViewModel
                {
                    Id = property.Id,
                    Code = property.Code,
                    PropertyTypeId = property.PropertyTypeId,
                    PropertyTypeName = propertyType.Name,
                    SaleTypeId = property.SaleTypeId,
                    SaleTypeName = saleType.Name,
                    Price = property.Price,
                    Meters = property.Meters,
                    Description = property.Description,
                    Bedrooms = property.Bedrooms,
                    Bathrooms = property.Bathrooms,
                    PropertyImages = propertyImages,
                    Improvements = propertyImprovements,
                    CreatedDate = property.CreatedDate,

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
        #endregion

        #region GetByIdViewModel Overriden
        /// <summary>
        /// Retrieves the property view model with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the property.</param>
        /// <returns>The property view model.</returns>
        public async override Task<PropertyViewModel> GetByIdViewModel(int id)
        {
            var properties = await GetAllViewModel();
            return properties.Where(p => p.Id == id).FirstOrDefault();

        }
        #endregion

        #region GetByIdSaveViewModel Overriden
        /// <summary>
        /// Retrieves the property view model with the specified ID and converts it to a save property view model.
        /// </summary>
        /// <param name="id">The ID of the property.</param>
        /// <returns>The save property view model.</returns>
        public async override Task<SavePropertyViewModel> GetByIdSaveViewModel(int id)
        {
            var property = await GetByIdViewModel(id);

            List<int> propertyImprovements = new List<int>();
            foreach (var improvement in property.Improvements)
            {
                var getImprovement = await _improvementService.GetByNameViewModel(improvement);
                propertyImprovements.Add(getImprovement.Id);

            }

            SavePropertyViewModel vm = new SavePropertyViewModel
            {
                Id = property.Id,
                Code = property.Code,
                Bathrooms = property.Bathrooms,
                Bedrooms = property.Bedrooms,
                Description = property.Description,
                Price = property.Price,
                Meters = property.Meters,
                SaleTypeId = property.SaleTypeId,
                PropertyTypeId = property.PropertyTypeId,
                AgentId = property.AgentId,
                Improvements = propertyImprovements,
                PropertyImages = property.PropertyImages
            };

            return vm;

        }
        #endregion

        #region GetPropertyByCode 
        /// <summary>
        /// Retrieves the property view model with the specified code.
        /// </summary>
        /// <param name="code">The code of the property.</param>
        /// <returns>The property view model.</returns>
        public async Task<PropertyViewModel> GetPropertyByCode(string code)
        {
            var propertiesList = await GetAllViewModel();
            var thisProperty = propertiesList.FirstOrDefault(sa => sa.Code == code);

            return thisProperty;

        }
        #endregion

        #region GetAgentProperties
        /// <summary>
        /// Retrieves the list of property view models associated with the specified agent ID.
        /// </summary>
        /// <param name="id">The ID of the agent.</param>
        /// <returns>The list of property view models.</returns>
        public async Task<List<PropertyViewModel>> GetAgentProperties(int id)
        {
            var propertiesList = await GetAllViewModel();
            var agent = await _agentService.GetByIdViewModel(id);

            propertiesList = propertiesList.Where(p => p.AgentId == agent.Id).ToList();

            return propertiesList;
        }
        #endregion

        #region GetAllViewModelWIthFilters
        /// <summary>
        /// Retrieves the list of property view models with the specified filters.
        /// </summary>
        /// <param name="filter">The filter object containing the filter criteria.</param>
        /// <returns>The list of property view models.</returns>
        public async Task<List<PropertyViewModel>> GetAllViewModelWIthFilters(FilterPropertyViewModel filter)
        {
            var propertiesList = await GetAllViewModel();

            propertiesList = propertiesList
                .Where(p => !filter.MinPrice.HasValue || p.Price >= filter.MinPrice.Value)
                .Where(p => !filter.MaxPrice.HasValue || p.Price <= filter.MaxPrice.Value)
                .Where(p => !filter.PropertyTypeId.HasValue || p.PropertyTypeId == filter.PropertyTypeId.Value)
                .Where(p => !filter.Bedrooms.HasValue || p.Bedrooms == filter.Bedrooms.Value)
                .Where(p => !filter.Bathrooms.HasValue || p.Bathrooms == filter.Bathrooms.Value)
                .ToList();

            return propertiesList;
        }
        #endregion

        #endregion

        #region Private Methods
        /// <summary>
        /// Generates a unique code for a property.
        /// </summary>
        /// <returns>The generated code.</returns>
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


        /// <summary>
        /// Checks if a property with the specified code already exists.
        /// </summary>
        /// <param name="code">The code to check.</param>
        /// <returns>True if the property with the code exists, otherwise false.</returns>
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
