using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.Agent;
using RoyalState.Core.Application.ViewModels.Users;

namespace RoyalState.Presentation.WebApp.Controllers
{
    public class AgentController : Controller
    {
        private readonly IAgentService _agentService;
        private readonly IPropertyService _propertyService;
        private readonly IFileService _fileService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse authViewModel;

        public AgentController(IAgentService agentService, IHttpContextAccessor httpContextAccessor, IFileService fileService, IPropertyService propertyService)
        {
            _agentService = agentService;
            _httpContextAccessor = httpContextAccessor;
            authViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _fileService = fileService;
            _propertyService = propertyService;
        }

        #region Agent Index
        public async Task <IActionResult> Index()
        {
            var agent = await _agentService.GetByUserIdViewModel(authViewModel.Id);
            var properties = await _propertyService.GetAgentProperties(agent.Id);
            return View(properties);

        }
        #endregion

        #region Get Agents
        public async Task<IActionResult> AllAgents()
        {
            var agents = await _agentService.GetAllViewModel();
            return View(agents);
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
