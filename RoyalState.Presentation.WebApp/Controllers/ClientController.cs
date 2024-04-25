using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.ClientProperties;
using RoyalState.Core.Application.ViewModels.Property;
using System.Text.Json;

namespace RoyalState.Presentation.WebApp.Controllers
{
    [Authorize(Roles = "Client")]
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IAccountService _accountService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly IPropertyService _propertyService;
        private readonly IFileService _fileService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse authViewModel;

        public ClientController(IClientService clientService, IHttpContextAccessor httpContextAccessor, IFileService fileService, IPropertyService propertyService, IPropertyTypeService propertyTypeService)

        {
            _clientService = clientService;
            _httpContextAccessor = httpContextAccessor;

            authViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");

            _fileService = fileService;
            _propertyService = propertyService;
            _propertyTypeService = propertyTypeService;
        }

        #region Client Index
        public async Task<IActionResult> Index(string? error, bool? favorite)
        {
            if (error != null)
            {
                ViewBag.Error = error;
            }
            if (favorite != null)
            {
                ViewBag.NewFavorite = favorite;
            }

            var propertyViewModel = new PropertyViewModel();

            if (TempData["PropertyViewModel"] != null)
            {
                var json = TempData["PropertyViewModel"].ToString();
                propertyViewModel = JsonSerializer.Deserialize<PropertyViewModel>(json);
            }

            bool? isEmpty = TempData.Peek("IsEmpty") as bool?;

            TempData.Remove("PropertyViewModel");
            TempData.Remove("IsEmpty");

            if (isEmpty != null)
            {
                ViewBag.PropertyTypes = await _propertyTypeService.GetAllViewModel();
                ViewBag.isEmpty = isEmpty;
                if (propertyViewModel != null)
                {
                    var propertiesHome = new List<PropertyViewModel> { propertyViewModel };
                    return View(propertiesHome);
                }
            }

            var propertyTypes = await _propertyTypeService.GetAllViewModel();
            ViewBag.PropertyTypes = propertyTypes;

            var properties = await _propertyService.GetAllViewModel();
            return View(properties);
        }
        #endregion

        #region My Favorites
        public async Task<IActionResult> MyFavorites()
        {
            var client = await _clientService.GetByUserIdViewModel(authViewModel.Id);
            var properties = await _clientService.GetFavoritePropertiesViewModel(client.Id);

            ViewBag.PropertyTypes = await _propertyTypeService.GetAllViewModel();
            return View(properties);
        }
        #endregion

        #region Mark Property As Favorite
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
                return RedirectToAction("Index", "Client", new { favorite = true });
            }
            else
            {
                await _clientService.DeleteFavorite(propertyId);

                var error = "Property deleted from your favorites.";
                return RedirectToAction("Index", "Client", new { error });
            }

        }
        #endregion

        #region Delete Favorite Property
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
        #endregion

    }
}
