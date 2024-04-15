using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.SaleTypes;

namespace RoyalState.Presentation.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SaleTypeController : Controller
    {
        private readonly ISaleTypeService _saleTypeService;

        public SaleTypeController(ISaleTypeService saleTypeService)
        {
            _saleTypeService = saleTypeService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _saleTypeService.GetAllViewModel());
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
            await _saleTypeService.Delete(id);

            return RedirectToRoute(new { controller = "SaleType", action = "Index" });
        }
        #endregion
    }
}
