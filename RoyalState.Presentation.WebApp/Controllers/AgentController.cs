using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.Agent;
using RoyalState.Core.Application.ViewModels.Property;
using RoyalState.Core.Application.ViewModels.Users;

namespace RoyalState.Presentation.WebApp.Controllers
{
    public class AgentController : Controller
    {
        private readonly IAgentService _agentService;
        private readonly IFileService _fileService;
        private readonly IImprovementService _improvmentService;
        private readonly IPropertyService _propertyService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly ISaleTypeService _saleTypeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse authViewModel;

        public AgentController(IAgentService agentService, IHttpContextAccessor httpContextAccessor, IImprovementService improvmentService, ISaleTypeService saleTypeService, IPropertyTypeService propertyTypeService, IPropertyService propertyService, IFileService fileService)
        {
            _agentService = agentService;
            _httpContextAccessor = httpContextAccessor;
            authViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _improvmentService = improvmentService;
            _saleTypeService = saleTypeService;
            _propertyTypeService = propertyTypeService;
            _propertyService = propertyService;
            _fileService = fileService;
        }

        #region Get Agents
        public async Task<IActionResult> AllAgents()
        {
            var agents = await _agentService.GetAllViewModel();
            return View(agents);
        }
        #endregion

        #region Edit
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
                vm.ImageUrl = await _fileService.UploadFileAsync(vm.File, authViewModel.Email, true, vm.ImageUrl);
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

        #region Dashboard
        public async Task<IActionResult> Maintenance()
        {
            var properties = await _propertyService.GetAllViewModel();
            return View(properties);
        }

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

        private async Task SetViewBagData()
        {
            ViewBag.SaleTypes = await _saleTypeService.GetAllViewModel();
            ViewBag.PropertyTypes = await _propertyTypeService.GetAllViewModel();
            ViewBag.Improvements = await _improvmentService.GetAllViewModel();
        }

        #endregion

        #region Search
        public async Task<IActionResult> SearchAgent(string agentName)
        {
            var agents = await _agentService.GetByNameViewModel(agentName);
            bool isEmpty = agents == null;

            ViewBag.IsEmpty = isEmpty;
            return View("AllAgents", agents != null ? agents : new List<AgentViewModel>());
        }
        #endregion

    }
}
