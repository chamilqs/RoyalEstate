using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.Property;

namespace RoyalState.Presentation.WebApp.Controllers
{
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

            List<int> propertyImprovements = new List<int>();
            foreach (var improvement in property.Improvements)
            {
                var getImprovement = await _improvmentService.GetByNameViewModel(improvement);
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
                if (vm.PropertyImages == null) { vm.PropertyImages = new List<string>(); };
                foreach (var file in files)
                {
                    var filename = await _fileService.UploadFileAsync(file, authViewModel.Email);
                    vm.PropertyImages.Add(filename);
                }
            }

            vm.PropertyImages?.RemoveAll(image => image == null);

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

        #region PropertyDetails
        public async Task<IActionResult> PropertyDetails(int id)
        {
            var property = await _propertyService.GetByIdViewModel(id);
            return View(property);
        }
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
