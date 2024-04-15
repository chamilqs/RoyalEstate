using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.PropertyTypes;

namespace RoyalState.Presentation.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PropertyTypeController : Controller
    {
        private readonly IPropertyTypeService _propertyTypeService;

        public PropertyTypeController(IPropertyTypeService propertyTypeService)
        {
            _propertyTypeService = propertyTypeService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _propertyTypeService.GetAllViewModel());
        }

        #region Create
        public async Task<IActionResult> Create()
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
            await _propertyTypeService.Delete(id);

            return RedirectToRoute(new { controller = "PropertyType", action = "Index" });
        }
        #endregion
    }
}
