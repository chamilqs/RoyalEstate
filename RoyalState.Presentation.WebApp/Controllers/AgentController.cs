using Microsoft.AspNetCore.Mvc;

namespace RoyalState.Presentation.WebApp.Controllers
{
    public class AgentController : Controller
    {
        public IActionResult AllAgents()
        {
            return View();
        }
    }
}
