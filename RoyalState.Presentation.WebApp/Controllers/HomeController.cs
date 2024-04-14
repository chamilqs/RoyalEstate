using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Enums;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.Agent;
using RoyalState.Core.Application.ViewModels.Property;

namespace RoyalState.Presentation.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly IAgentService _agentService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse authViewModel;

        public HomeController(IHttpContextAccessor httpContextAccessor, IPropertyService propertyService, IAgentService agentService, IPropertyTypeService propertyTypeService)
        {
            _httpContextAccessor = httpContextAccessor;
            _propertyTypeService = propertyTypeService;
            _propertyService = propertyService;
            authViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _agentService = agentService;
        }

        #region Index
        public async Task<IActionResult> Index()
        {
            var properties = await _propertyService.GetAllViewModel();
            var propertyTypes = await _propertyTypeService.GetAllViewModel();

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

            ViewBag.PropertyTypes = propertyTypes;
            return View(properties);
        }
        #endregion

        #region Search

        #region SearchPropertyByCode
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
            return View("Index", properties != null ? properties : new List<PropertyViewModel>());
        }
        #endregion

        #region SearchAgentByName
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
        public async Task<IActionResult> AgentProperties(int id)
        {
            var properties = await _propertyService.GetAgentProperties(id);

            var firstProperty = properties[0];

            AgentViewModel agent = new()
            {
                Id = firstProperty.Id,
                FirstName = firstProperty.AgentFirstName,
                LastName = firstProperty.AgentLastName,
                Email = firstProperty.AgentEmail,
                Phone = firstProperty.AgentPhone,
                ImageUrl = firstProperty.AgentImage,
            };

            ViewBag.Agent = agent;
            return View("AgentProperties", properties);
        }
        #endregion

    }
}
