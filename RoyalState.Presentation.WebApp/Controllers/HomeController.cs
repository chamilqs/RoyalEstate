using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Enums;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.Property;

namespace RoyalState.Presentation.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse authViewModel;

        public HomeController(IHttpContextAccessor httpContextAccessor, IPropertyService propertyService)
        {
            _httpContextAccessor = httpContextAccessor;
            _propertyService = propertyService;
            authViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
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


    }
}
