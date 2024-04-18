using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.ClientProperties;
using RoyalState.Core.Application.ViewModels.Property;

namespace RoyalState.Presentation.WebApp.Controllers
{
    [Authorize(Roles = "Client")]
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;
#pragma warning disable CS0169 // The field 'ClientController._accountService' is never used
        private readonly IAccountService _accountService;
#pragma warning restore CS0169 // The field 'ClientController._accountService' is never used
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly IPropertyService _propertyService;
        private readonly IFileService _fileService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse authViewModel;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ClientController(IClientService clientService, IHttpContextAccessor httpContextAccessor, IFileService fileService, IPropertyService propertyService, IPropertyTypeService propertyTypeService)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _clientService = clientService;
            _httpContextAccessor = httpContextAccessor;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            authViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            _fileService = fileService;
            _propertyService = propertyService;
            _propertyTypeService = propertyTypeService;
        }

        #region Client Index
        public async Task<IActionResult> Index(string? error, List<PropertyViewModel>? propertiesHome, bool? isEmpty, bool? favorite)
        {
            if (error != null)
            {
                ViewBag.Error = error;
            }
            if (propertiesHome != null && propertiesHome.Count != 0)
            {
                return View(propertiesHome);
            }
            if (isEmpty != null)
            {
                ViewBag.isEmpty = isEmpty;
            }
            if (favorite != null)
            {
                ViewBag.NewFavorite = favorite;
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

        #region Search

        #region SearchPropertyByFilters
        [HttpPost]
        public async Task<IActionResult> SearchFavoritesByFilters(int? propertyTypeId, double? maxPrice, double? minPrice, int? roomsNumber, int? bathsNumber)
        {
            var propertyTypes = await _propertyTypeService.GetAllViewModel();
            FilterPropertyViewModel filter = new()
            {
                PropertyTypeId = propertyTypeId,
                MaxPrice = maxPrice,
                MinPrice = minPrice,
                Bedrooms = roomsNumber,
                Bathrooms = bathsNumber
            };

            var properties = await _propertyService.GetAllViewModelWIthFilters(filter);
            bool isEmpty = properties == null || properties.Count == 0;

            ViewBag.IsEmpty = isEmpty;
            ViewBag.PropertyTypes = propertyTypes;
            return View("MyFavorites", properties ?? new List<PropertyViewModel>());
        }
        #endregion

        #region SearchPropertyByCode
        public async Task<IActionResult> SearchProperty(string code)
        {
            var propertyTypes = await _propertyTypeService.GetAllViewModel();
            var property = await _propertyService.GetPropertyByCode(code);

            ViewBag.PropertyTypes = propertyTypes;

            bool isEmpty = property == null;
            ViewBag.IsEmpty = isEmpty;
#pragma warning disable CS8604 // Possible null reference argument.
            List<PropertyViewModel> properties = isEmpty ? new List<PropertyViewModel>() : new List<PropertyViewModel> { property };
#pragma warning restore CS8604 // Possible null reference argument.

            return View("MyFavorites", properties);
        }
        #endregion

        #endregion
    }
}
