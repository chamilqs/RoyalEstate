using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.Property;
using RoyalState.Core.Application.ViewModels.Users;

namespace RoyalState.Presentation.WebApp.Controllers
{
    [Authorize(Roles = "Agent")]
    public class AgentController : Controller
    {
        private readonly IAgentService _agentService;
        private readonly IPropertyService _propertyService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly IFileService _fileService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse authViewModel;

        public AgentController(IAgentService agentService, IHttpContextAccessor httpContextAccessor, IFileService fileService, IPropertyService propertyService, IPropertyTypeService propertyTypeService)
        {
            _agentService = agentService;
            _httpContextAccessor = httpContextAccessor;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            authViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            _fileService = fileService;
            _propertyService = propertyService;
            _propertyTypeService = propertyTypeService;
        }

        #region Agent Index
        public async Task<IActionResult> Index(List<PropertyViewModel>? propertiesHome, bool? isEmpty)
        {
            if (propertiesHome != null && propertiesHome.Count != 0)
            {
                return View(propertiesHome);
            }

            if (isEmpty != null)
            {
                ViewBag.isEmpty = isEmpty;
            }

            var agent = await _agentService.GetByUserIdViewModel(authViewModel.Id);
            var properties = await _propertyService.GetAgentProperties(agent.Id);

            ViewBag.PropertyTypes = await _propertyTypeService.GetAllViewModel();
            return View(properties);

        }
        #endregion

        #region SearchPropertyByFilters
        [HttpPost]
        public async Task<IActionResult> SearchPropertyByFilters(int? propertyTypeId, double? maxPrice, double? minPrice, int? roomsNumber, int? bathsNumber)
        {
            var propertyTypes = await _propertyTypeService.GetAllViewModel();
            FilterPropertyViewModel filter = new()
            {
                PropertyTypeId = propertyTypeId,
                MaxPrice = maxPrice,
                MinPrice = minPrice,
                Bedrooms = roomsNumber,
                Bathrooms = bathsNumber
            };

            var properties = await _propertyService.GetAllViewModelWIthFilters(filter);
            bool isEmpty = properties == null || properties.Count == 0;

            ViewBag.IsEmpty = isEmpty;
            ViewBag.PropertyTypes = propertyTypes;
            return View("Index", properties ?? new List<PropertyViewModel>());
        }
        #endregion

        #region Edit Profile
        public async Task<IActionResult> EditProfile()
        {
            SaveUserViewModel vm = await _agentService.GetProfileDetails();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(SaveUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Please check for missing or incorrect fields";
                return View("EditProfile", vm);
            }

            if (vm.File != null)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                vm.ImageUrl = await _fileService.UploadFileAsync(vm.File, authViewModel.Email, true, vm.ImageUrl);
#pragma warning restore CS8604 // Possible null reference argument.
            }

            UpdateUserResponse response = new();
            response = await _agentService.UpdateAsync(vm);

            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View("EditProfile", vm);
            }

            return RedirectToAction("EditProfile");
        }
        #endregion        

    }
}
