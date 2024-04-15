using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.Services;
using RoyalState.Core.Application.ViewModels.Admins;
using RoyalState.Core.Application.ViewModels.Users;

namespace RoyalState.Presentation.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
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
            ViewBag.User = _authViewModel;
            return View(await _adminService.GetAllViewModel());
        }

        #region Dashboard
        public async Task<IActionResult> Dashboard()
        {
            return View(await _adminService.Dashboard());
        }
        #endregion

        #region Create
        public async Task<IActionResult> Create()
        {
            return View("RegisterAdmin", new SaveUserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveUserViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("RegisterAdmin", vm);

            var origin = Request.Headers["origin"];
            RegisterResponse response = await _adminService.Add(vm, origin);

            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View("RegisterAdmin", vm);
            }

            return RedirectToRoute(new { controller = "Admin", action = "Index" });
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(int id)
        {
            AdminViewModel admin = await _adminService.GetByIdViewModel(id);
            SaveUserViewModel vm = new()
            {
                Id = admin.UserId,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                UserName = admin.Username,
                Email = admin.Email,
                Identification = admin.Identification,
                Role = (int)Roles.Admin,
            };

            return View("UpdateAdmin", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveUserViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("UpdateAdmin", vm);

            var response = await _adminService.Update(vm);

            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View("UpdateAdmin", vm);
            }

            return RedirectToRoute(new { controller = "Admin", action = "Index" });
        }
        #endregion

        #region Active & Unactive User
        [HttpPost]
        public async Task<IActionResult> UpdateUserStatus(string username, string controller)
        {
            var response = await _adminService.UpdateUserStatus(username);

            if (response.HasError)
            {
                return RedirectToRoute(new { controller = controller, action = "Index", hasError = response.HasError, message = response.Error });
            }

            return RedirectToRoute(new { controller = controller, action = "Index", });
        }
        #endregion
    }
}
