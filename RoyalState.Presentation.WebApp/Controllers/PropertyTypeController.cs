using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.PropertyTypes;

namespace RoyalState.Presentation.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PropertyTypeController : Controller
    {
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly IPropertyService _propertyService;
        private readonly IFileService _fileService;

        public PropertyTypeController(IPropertyTypeService propertyTypeService, IPropertyService propertyService, IFileService fileService)
        {
            _propertyTypeService = propertyTypeService;
            _propertyService = propertyService;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _propertyTypeService.GetAllViewModelWithInclude());
        }

        #region Create
        public IActionResult Create()
        {
            return View("SavePropertyType", new SavePropertyTypeViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SavePropertyTypeViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("SavePropertyType", vm);

            await _propertyTypeService.Add(vm);

            return RedirectToRoute(new { controller = "PropertyType", action = "Index" });
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(int id)
        {
            SavePropertyTypeViewModel vm = await _propertyTypeService.GetByIdSaveViewModel(id);

            return View("SavePropertyType", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SavePropertyTypeViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("SavePropertyType", vm);

            await _propertyTypeService.Update(vm, vm.Id);

            return RedirectToRoute(new { controller = "PropertyType", action = "Index" });
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int id)
        {
            return View(await _propertyTypeService.GetByIdViewModel(id));
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int id)
        {

            try
            {
                var properties = await _propertyService.GetPropertiesByPropertyType(id);

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

            await _propertyTypeService.Delete(id);

            return RedirectToRoute(new { controller = "PropertyType", action = "Index" });
        }
        #endregion
    }
}
