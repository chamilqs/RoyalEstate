using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.Services;
using RoyalState.Core.Application.ViewModels.ClientProperties;
using RoyalState.Core.Application.ViewModels.Property;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public async Task<IActionResult> Index(string? error, List<PropertyViewModel>? propertiesHome, bool? isEmpty, bool? favorite)
        {
            if (error != null)
            {
                ViewBag.Error = error;
            }
            if (propertiesHome != null && propertiesHome.Count() != 0)
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
                return RedirectToAction("Index", "Client", new { error = error });
            }

            return RedirectToAction("Index");
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
            FilterPropertyViewModel filter = new FilterPropertyViewModel();
            filter.PropertyTypeId = propertyTypeId;
            filter.MaxPrice = maxPrice;
            filter.MinPrice = minPrice;
            filter.Bedrooms = roomsNumber;
            filter.Bathrooms = bathsNumber;

            var properties = await _propertyService.GetAllViewModelWIthFilters(filter);
            bool isEmpty = properties == null || properties.Count() == 0;

            ViewBag.IsEmpty = isEmpty;
            ViewBag.PropertyTypes = propertyTypes;
            return View("MyFavorites", properties != null ? properties : new List<PropertyViewModel>());
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
            List<PropertyViewModel> properties = isEmpty ? new List<PropertyViewModel>() : new List<PropertyViewModel> { property };

            return View("MyFavorites", properties);
        }
        #endregion

        #endregion
    }
}
