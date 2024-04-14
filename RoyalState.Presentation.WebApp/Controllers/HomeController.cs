using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Enums;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.Services;
using RoyalState.Core.Application.ViewModels.Agent;
using RoyalState.Core.Application.ViewModels.Property;

namespace RoyalState.Presentation.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IAgentService _agentService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse authViewModel;

        public HomeController(IHttpContextAccessor httpContextAccessor, IPropertyService propertyService, IAgentService agentService)
        {
            _httpContextAccessor = httpContextAccessor;
            _propertyService = propertyService;
            authViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _agentService = agentService;
        }

        public async Task<IActionResult> Index()
        {
            var properties = await _propertyService.GetAllViewModel();

            if (authViewModel != null)
            {
                if (authViewModel.Roles.Any(role => role == Roles.Admin.ToString()))
                {
                    return RedirectToAction("Index", Roles.Admin.ToString());
                }
                else if (authViewModel.Roles.Any(role => role == Roles.Agent.ToString()))
                {
                    return RedirectToAction("Index", Roles.Agent.ToString());
                }
                else if (authViewModel.Roles.Any(role => role == Roles.Client.ToString()))
                {
                    return RedirectToAction("Index", Roles.Client.ToString());
                }
            }

            return View(properties);
        }

        #region Search

        #region SearchProperty
        public async Task<IActionResult> SearchProperty(string code)
        {
            var property = await _propertyService.GetPropertyByCode(code);
            bool isEmpty = property == null;

            ViewBag.IsEmpty = isEmpty;

            List<PropertyViewModel> properties = new();
            if (!isEmpty)
            {
                properties.Add(property);
            }

            return View("Index", property != null ? properties : new List<PropertyViewModel>());

        }
        #endregion

        #region SearchAgent
        public async Task<IActionResult> SearchAgent(string agentName)
        {
            var agents = await _agentService.GetByNameViewModel(agentName);
            bool isEmpty = agents == null;

            ViewBag.IsEmpty = isEmpty;
            return View("AllAgents", agents != null ? agents : new List<AgentViewModel>());
        }
        #endregion

        #endregion

        #region PropertyDetails
        public async Task<IActionResult> PropertyDetails(int id)
        {
            var property = await _propertyService.GetByIdViewModel(id);
            return View(property);
        }
        #endregion        

        #region Get Agents
        public async Task<IActionResult> AllAgents()
        {
            var agents = await _agentService.GetAllViewModel();
            return View(agents);
        }
        #endregion

    }
}
