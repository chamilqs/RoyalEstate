using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.ClientProperties;

namespace RoyalState.Presentation.WebApp.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IAccountService _accountService;
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

        public async Task<IActionResult> Index(string? error)
        {
            if (error != null)
            {
                ViewBag.Error = error;
            }
            var properties = await _propertyService.GetAllViewModel();
            return View(properties);
        }
        public async Task<IActionResult> MyFavorites()
        {
            var client = await _clientService.GetByUserIdViewModel(authViewModel.Id);
            var properties = await _clientService.GetFavoritePropertiesViewModel(client.Id);
            return View(properties);
        }

        [HttpPost]
        public async Task<IActionResult> MarkPropertyAsFavorite(int propertyId)
        {
            var client = await _clientService.GetByUserIdViewModel(authViewModel.Id);
            var clientFavoritePropertyIds = await _clientService.GetIdsOfFavoriteProperties(client.Id);

            if (!clientFavoritePropertyIds.Contains(propertyId))
            {
                var clientProperty = new SaveClientPropertiesViewModel
                {
                    ClientId = client.Id,
                    PropertyId = propertyId
                };

                await _clientService.MarkPropertyAsFavorite(clientProperty);
            }
            else
            {
                var error = "This property is already in your favorites";
                return RedirectToAction("Index", "Client", new { error = error });
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteFavorite(int id)
        {
            var property = await _propertyService.GetByIdSaveViewModel(id);

            return View(property);
        }        

        [HttpPost]
        public async Task<IActionResult> DeleteFavoritePost(int id)
        {

            await _clientService.DeleteFavorite(id);
            return RedirectToAction("MyFavorites");
        }


    }
}
