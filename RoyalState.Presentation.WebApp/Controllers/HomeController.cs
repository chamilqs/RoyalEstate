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
        public async Task<IActionResult> Index(List<PropertyViewModel>? propertiesHome, bool? isEmpty)
        {
            var properties = await _propertyService.GetAllViewModel();
            var propertyTypes = await _propertyTypeService.GetAllViewModel();
            ViewBag.PropertyTypes = propertyTypes;

            if (propertiesHome != null && propertiesHome.Count > 0)
            {
                return View(propertiesHome);
            }

            if (isEmpty != null)
            {
                ViewBag.isEmpty = isEmpty;
            }

            if (authViewModel != null)
            {
                var roles = authViewModel.Roles;
                if (roles.Contains(Roles.Admin.ToString()))
                {
                    return RedirectToAction("Dashboard", Roles.Admin.ToString());
                }
                else if (roles.Contains(Roles.Agent.ToString()))
                {
                    return RedirectToAction("Index", Roles.Agent.ToString());
                }
                else if (roles.Contains(Roles.Client.ToString()))
                {
                    return RedirectToAction("Index", Roles.Client.ToString());
                }
            }


            return View(properties);
        }



        #endregion

        #region Search

        #region SearchPropertyByCode
        public async Task<IActionResult> SearchProperty(string code)
        {
            var property = await _propertyService.GetPropertyByCode(code);

            bool isEmpty = property == null;
#pragma warning disable CS8604 // Possible null reference argument.
            List<PropertyViewModel> propertiesHome = isEmpty ? new List<PropertyViewModel>() : new List<PropertyViewModel> { property };
#pragma warning restore CS8604 // Possible null reference argument.

            if (authViewModel != null)
            {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                string role = authViewModel.Roles.FirstOrDefault()?.ToString();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                if (!string.IsNullOrEmpty(role))
                {
                    return RedirectToRoute(new { controller = role, action = "Index", propertiesHome, isEmpty });
                }
            }

            return RedirectToAction("Index", new { propertiesHome, isEmpty });
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

        #region SearchAgentByName
        public async Task<IActionResult> SearchAgent(string agentName)
        {
            var agents = await _agentService.GetByNameViewModel(agentName);
            bool isEmpty = agents == null;

            ViewBag.IsEmpty = isEmpty;
            return View("AllAgents", agents ?? new List<AgentViewModel>());
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

            if (properties == null || !properties.Any())
            {
                return View("AgentProperties", new List<PropertyViewModel>());
            }

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
