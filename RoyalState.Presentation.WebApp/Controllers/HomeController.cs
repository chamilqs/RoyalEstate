using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.DTOs.Account;

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
             //Send viewbags with data about property types etc if authviewmodel is not null 
            if (authViewModel != null)
            {
                // ViewBag.PropertyTypes = authViewModel;
            }
            else
            {
                Console.WriteLine("authViewModel is null");
            }
            return View();
        }

    }
}
