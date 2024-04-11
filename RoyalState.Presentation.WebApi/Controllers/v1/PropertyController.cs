using Microsoft.AspNetCore.Mvc;

namespace RoyalState.Presentation.WebApi.Controllers.v1
{
    public class PropertyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
