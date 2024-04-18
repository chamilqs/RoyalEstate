using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.Property;

namespace RoyalState.Presentation.WebApp.Controllers
{
    [Authorize(Roles = "Agent")]
    public class PropertyController : Controller
    {
        #region Fields
        private readonly IFileService _fileService;
        private readonly IAgentService _agentService;
        private readonly IImprovementService _improvmentService;
        private readonly IPropertyService _propertyService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly ISaleTypeService _saleTypeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse authViewModel;
        #endregion

        #region Constructor
        public PropertyController(IFileService fileService, IAgentService agentService, IHttpContextAccessor httpContextAccessor, IImprovementService improvmentService, ISaleTypeService saleTypeService, IPropertyTypeService propertyTypeService, IPropertyService propertyService)
        {
            _fileService = fileService;
            _agentService = agentService;
            _httpContextAccessor = httpContextAccessor;
            authViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _improvmentService = improvmentService;
            _saleTypeService = saleTypeService;
            _propertyTypeService = propertyTypeService;
            _propertyService = propertyService;
        }
        #endregion

        #region Functionalities & Views

        #region Maintenance
        public async Task<IActionResult> Maintenance()
        {
            var agent = await _agentService.GetByUserIdViewModel(authViewModel.Id);
            var properties = await _propertyService.GetAgentProperties(agent.Id);

            ViewBag.PropertyTypes = await _propertyTypeService.GetAllViewModel();

            return View(properties);
        }
        #endregion

        #region NewProperty
        public async Task<IActionResult> NewProperty()
        {

            await SetViewBagData();
            return View(new SavePropertyViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> NewPropertyPost(SavePropertyViewModel vm, IFormFileCollection files)
        {
            string error = null;

            if (!ModelState.IsValid)
            {
                error = "Please check for missing or incorrect fields";
            }
            else if (vm.Improvements == null || !vm.Improvements.Any())
            {
                error = "You need to provide at least one improvement for the property.";
            }
            else if (!files.Any())
            {
                error = "You need to provide at least one image for the property.";
            }
            else if (files.Count > 4)
            {
                error = "The maximum number of property images allowed is 4.";
            }

            if (error != null)
            {
                await SetViewBagData();
                ViewBag.Error = error;
                return View("NewProperty", vm);
            }

            var agent = await _agentService.GetByUserIdViewModel(authViewModel.Id);
            vm.AgentId = agent.Id;

            vm.PropertyImages = new List<string>();
            foreach (var file in files)
            {
                var filename = await _fileService.UploadFileAsync(file, authViewModel.Email);
                vm.PropertyImages.Add(filename);
            }

            await _propertyService.Add(vm);
            return RedirectToAction("Maintenance");
        }
        #endregion

        #region EditProperty
        public async Task<IActionResult> EditProperty(int id)
        {
            var property = await _propertyService.GetByIdViewModel(id);

            List<int> propertyImprovements = new();
            foreach (var improvement in property.Improvements)
            {
                var getImprovement = await _improvmentService.GetByNameViewModel(improvement);
                propertyImprovements.Add(getImprovement.Id);

            }

            SavePropertyViewModel vm = new()
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

            await SetViewBagData();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditPropertyPost(SavePropertyViewModel vm, IFormFileCollection files)
        {
            string error = null;

            if (!ModelState.IsValid)
            {
                error = "Please check for missing or incorrect fields";
            }
            else if (vm.Improvements == null || !vm.Improvements.Any())
            {
                error = "You need to provide at least one improvement for the property.";
            }

            if (error != null)
            {
                await SetViewBagData();
                ViewBag.Error = error;
                return View("EditProperty", vm);
            }

            if (files.Any())
            {
                vm.PropertyImages ??= new List<string>();

                var fileCount = 0;
                foreach (var file in files)
                {
                    if (fileCount >= 4)
                    {
                        break;
                    }

                    var filename = await _fileService.UploadFileAsync(file, authViewModel.Email);
                    vm.PropertyImages.Add(filename);
                    fileCount++;
                }
            }

            vm.PropertyImages?.RemoveAll(image => image == null);

            if (vm.PropertyImages.Count > 4)
            {
                error = "The maximum number of property images allowed is 4.";
                await SetViewBagData();
                ViewBag.Error = error;
                return View("EditProperty", vm);
            }

            await _propertyService.Update(vm, vm.Id);
            return RedirectToAction("Maintenance");
        }
        #endregion

        #region DeleteProperty
        public async Task<IActionResult> DeleteProperty(int id)
        {
            var vm = await _propertyService.GetByIdSaveViewModel(id);

            await SetViewBagData();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePropertyPost(int id)
        {
            await _propertyService.Delete(id);
            return RedirectToAction("Maintenance");
        }
        #endregion

        #region Search

        #region SearchPropertyByFilters
        [HttpPost]
        public async Task<IActionResult> SearchPropertyByFilters(int? propertyTypeId, double? maxPrice, double? minPrice, int? roomsNumber, int? bathsNumber)
        {
            var propertyTypes = await _propertyTypeService.GetAllViewModel();
            FilterPropertyViewModel filter = new FilterPropertyViewModel();
            filter.PropertyTypeId = propertyTypeId;
            filter.MaxPrice = maxPrice;
            filter.MinPrice = minPrice;
            filter.Bedrooms = roomsNumber;
            filter.Bathrooms = bathsNumber;

            var properties = await _propertyService.GetAllViewModelWIthFilters(filter);
            bool isEmpty = properties == null || properties.Count() == 0;

            ViewBag.IsEmpty = isEmpty;
            ViewBag.PropertyTypes = propertyTypes;
            return View("Maintenance", properties != null ? properties : new List<PropertyViewModel>());
        }
        #endregion

        #region SearchPropertyByCode
        public async Task<IActionResult> SearchProperty(string code)
        {
            var propertyTypes = await _propertyTypeService.GetAllViewModel();
            var property = await _propertyService.GetPropertyByCode(code);

            ViewBag.PropertyTypes = propertyTypes;

            bool isEmpty = property == null;
            ViewBag.IsEmpty = isEmpty;
            List<PropertyViewModel> properties = isEmpty ? new List<PropertyViewModel>() : new List<PropertyViewModel> { property };

            return View("Maintenance", properties);
        }
        #endregion

        #endregion

        #endregion

        #region Private Methods
        private async Task SetViewBagData()
        {
            ViewBag.SaleTypes = await _saleTypeService.GetAllViewModel();
            ViewBag.PropertyTypes = await _propertyTypeService.GetAllViewModel();
            ViewBag.Improvements = await _improvmentService.GetAllViewModel();
        }
        #endregion
    }
}
