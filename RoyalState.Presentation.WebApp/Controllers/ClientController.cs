using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.Interfaces.Services;

namespace RoyalState.Presentation.WebApp.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IPropertyService _propertyService;
        private readonly IFileService _fileService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse authViewModel;
        public ClientController(IClientService clientService, IHttpContextAccessor httpContextAccessor, IFileService fileService, IPropertyService propertyService)
        {
            _clientService = clientService;
            _httpContextAccessor = httpContextAccessor;
            authViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _fileService = fileService;
            _propertyService = propertyService;

        }

        public async Task<IActionResult> Index()
        {
            var properties = await _propertyService.GetAllViewModel();

            return View(properties);
        }

        [HttpPost]
        public async Task<IActionResult> MarkPropertyAsFavorite()
        {

            return View();
        }



    }
}
