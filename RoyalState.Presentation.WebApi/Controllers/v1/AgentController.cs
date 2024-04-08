using Microsoft.AspNetCore.Mvc;

namespace RoyalState.Presentation.WebApi.Controllers.v1
{
    public class AgentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
