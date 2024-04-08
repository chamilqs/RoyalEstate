using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.Agent;

namespace RoyalState.Presentation.WebApp.Controllers
{
    public class AgentController : Controller
    { 
        private readonly IAgentService _agentService;

        public AgentController(IAgentService agentService)
        {
            _agentService = agentService;
        }

        public async Task<IActionResult> AllAgents()
        {
            var agents = await _agentService.GetAllViewModel();
            return View(agents);
        }

        public async Task<IActionResult> SearchAgent(string agentName)
        {
            var agents = await _agentService.GetByNameViewModel(agentName);
            bool isEmpty = agents == null;

            ViewBag.IsEmpty = isEmpty;
            return View("AllAgents", agents != null ? agents : new List<AgentViewModel>());
        }

        /*
        public async Task<IActionResult> AgentDetails(string userId)
        {
            var agent = await _agentService.GetByUserIdViewModel(userId);
            return View(agent);
        }*/


    }
}
