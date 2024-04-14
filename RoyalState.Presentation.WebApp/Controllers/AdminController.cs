using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Enums;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.Services;
using RoyalState.Core.Application.ViewModels.Admins;

namespace RoyalState.Presentation.WebApp.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAdminService _adminService;
        private readonly AuthenticationResponse _authViewModel;

        public AdminController(IHttpContextAccessor httpContextAccessor, IAdminService adminService)
        {
            _httpContextAccessor = httpContextAccessor;
            _authViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _adminService = adminService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        #region Create
        public async Task<IActionResult> Create()
        {
            return View("SaveAdmin", new SaveAdminViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveAdminViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("SaveAdmin", vm);

            await _adminService.Add(vm);

            return RedirectToRoute(new { controller = "Admin", action = "Index" });
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(int id)
        {
            SaveAdminViewModel vm = await _adminService.GetByIdSaveViewModel(id);

            return View("SaveAdmin", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveAdminViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("SaveAdmin", vm);

            await _adminService.Update(vm, vm.Id);

            return RedirectToRoute(new { controller = "Admin", action = "Index" });
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int id)
        {
            return View(await _adminService.GetByIdViewModel(id));
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int id)
        {
            await _adminService.Delete(id);

            return RedirectToRoute(new { controller = "Admin", action = "Index" });
        }
        #endregion


        #region Active & Unactive User
        [HttpPost]
        public async Task<IActionResult> UpdateUserStatus(string userId, string controller)
        {
            var response = await _adminService.UpdateUserStatus(userId);

            if (response.HasError)
            {
                return RedirectToRoute(new { controller = controller, action = "Index", hasError = response.HasError, message = response.Error });
            }

            return RedirectToRoute(new { controller = controller, action = "Index", });
        }
        #endregion
    }
}
