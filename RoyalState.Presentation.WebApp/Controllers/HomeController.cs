using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Enums;

namespace RoyalState.Presentation.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse authViewModel;

        public HomeController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            authViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public IActionResult Index()
        {
            if (authViewModel != null)
            {
                // ViewBag.PropertyTypes = authViewModel;
                Console.WriteLine("authViewModel is not null");

                if (authViewModel.Roles.Any(role => role == Roles.Admin.ToString()))
                {
                    return RedirectToAction("Index", Roles.Admin.ToString());
                }
            }
            else
            {
                Console.WriteLine("authViewModel is null");
            }

            return View();
        }

    }
}
