using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Enums;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.Admins;
using RoyalState.Core.Application.ViewModels.Users;

namespace RoyalState.Presentation.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAdminService _adminService;
        private readonly IPropertyService _propertyService;
        private readonly IAgentService _agentService;
        private readonly AuthenticationResponse _authViewModel;

        public AdminController(IHttpContextAccessor httpContextAccessor, IAdminService adminService, IAgentService agentService, IPropertyService propertyService)
        {
            _httpContextAccessor = httpContextAccessor;

            _authViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");

            _adminService = adminService;
            _agentService = agentService;
            _propertyService = propertyService;
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
        public IActionResult Create()
        {
            return View("RegisterAdmin", new SaveUserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveUserViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("RegisterAdmin", vm);

            var origin = Request.Headers["origin"];
#pragma warning disable CS8604 // Possible null reference argument.
            RegisterResponse response = await _adminService.Add(vm, origin);
#pragma warning restore CS8604 // Possible null reference argument.

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

        #region AgentList
        public async Task<IActionResult> AgentList()
        {
            return View(await _propertyService.GetAgentsWithPropertyQuantity());
        }
        #endregion

        #region Active & Unactive User
        [HttpPost]
        public async Task<IActionResult> UpdateUserStatus(string username, string action)
        {
            var response = await _adminService.UpdateUserStatus(username);

            if (response.HasError)
            {
                return RedirectToRoute(new { controller = "Admin", action = action == "UpdateUserStatus" ? "Index" : action, hasError = response.HasError, message = response.Error });
            }

            return RedirectToRoute(new { controller = "Admin", action = action == "UpdateUserStatus" ? "Index" : action });
        }
        #endregion

        #region DeleteAgent
        public async Task<IActionResult> DeleteAgent(int id)
        {
            return View(await _agentService.GetByIdViewModel(id));
        }

        [HttpPost]
        [ActionName("DeleteAgent")]
        public async Task<IActionResult> DeleteAgentPost(int id)
        {
            var response = await _adminService.DeleteAgent(id);

            if (response.HasError)
            {
                var errorRouteValues = new
                {
                    controller = "Admin",
                    action = "AgentList",
                    hasError = response.HasError,
                    message = response.Error
                };

                return RedirectToRoute(errorRouteValues);
            }

            return RedirectToRoute(new { controller = "Admin", action = "AgentList" });
        }
        #endregion

    }
}
