using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.Services;
using RoyalState.Core.Application.ViewModels.SaleTypes;

namespace RoyalState.Presentation.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SaleTypeController : Controller
    {
        private readonly ISaleTypeService _saleTypeService; 
        private readonly IPropertyService _propertyService;
        private readonly IPropertyImageService _propertyImageService;
        private readonly IFileService _fileService;

        public SaleTypeController(ISaleTypeService saleTypeService, IPropertyService propertyService, IPropertyImageService propertyImageService, IFileService fileService)
        {
            _saleTypeService = saleTypeService;
            _propertyService = propertyService;
            _propertyImageService = propertyImageService;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _saleTypeService.GetAllViewModelWithInclude());
        }

        #region Create
        public async Task<IActionResult> Create()
        {
            return View("SaveSaleType", new SaveSaleTypeViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveSaleTypeViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("SaveSaleType", vm);

            await _saleTypeService.Add(vm);

            return RedirectToRoute(new { controller = "SaleType", action = "Index" });
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(int id)
        {
            SaveSaleTypeViewModel vm = await _saleTypeService.GetByIdSaveViewModel(id);

            return View("SaveSaleType", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveSaleTypeViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("SaveSaleType", vm);

            await _saleTypeService.Update(vm, vm.Id);

            return RedirectToRoute(new { controller = "SaleType", action = "Index" });
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int id)
        {
            return View(await _saleTypeService.GetByIdViewModel(id));
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                var properties = await _propertyService.GetPropertiesBySaleType(id);

                foreach (var property in properties)
                {
                    foreach (var propertyImage in property.PropertyImages)
                    {
                        await _fileService.DeleteFileAsync(propertyImage);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error ocurred while trying to delete the images of the properties: {ex.Message}");
            }

            await _saleTypeService.Delete(id);

            return RedirectToRoute(new { controller = "SaleType", action = "Index" });
        }
        #endregion
    }
}
