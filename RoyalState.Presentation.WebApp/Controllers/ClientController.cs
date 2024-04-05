using Microsoft.AspNetCore.Mvc;

namespace RoyalState.Presentation.WebApp.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
