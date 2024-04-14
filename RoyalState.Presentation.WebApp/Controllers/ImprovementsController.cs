using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.Improvements;

namespace RoyalState.Presentation.WebApp.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class ImprovementController : Controller
    {
        private readonly IImprovementService _improvementService;

        public ImprovementController(IImprovementService improvementService)
        {
            _improvementService = improvementService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _improvementService.GetAllViewModel());
        }

        #region Create
        public async Task<IActionResult> Create()
        {
            return View("SaveImprovement", new SaveImprovementViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveImprovementViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("SaveImprovement", vm);

            await _improvementService.Add(vm);

            return RedirectToRoute(new { controller = "Improvement", action = "Index" });
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(int id)
        {
            SaveImprovementViewModel vm = await _improvementService.GetByIdSaveViewModel(id);

            return View("SaveImprovement", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveImprovementViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("SaveImprovement", vm);

            await _improvementService.Update(vm, vm.Id);

            return RedirectToRoute(new { controller = "Improvement", action = "Index" });
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int id)
        {
            return View(await _improvementService.GetByIdViewModel(id));
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int id)
        {
            await _improvementService.Delete(id);

            return RedirectToRoute(new { controller = "Improvement", action = "Index" });
        }
        #endregion
    }
}
