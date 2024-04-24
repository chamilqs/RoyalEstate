using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Enums;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.Developers;
using RoyalState.Core.Application.ViewModels.Users;

namespace RoyalState.Presentation.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DeveloperController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDeveloperService _developerService;
        private readonly AuthenticationResponse _authViewModel;

        public DeveloperController(IHttpContextAccessor httpContextAccessor, IDeveloperService developerService)
        {
            _httpContextAccessor = httpContextAccessor;

            _authViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");

            _developerService = developerService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.User = _authViewModel;
            return View(await _developerService.GetAllViewModel());
        }

        #region Create
        public IActionResult Create()
        {
            return View("RegisterDeveloper", new SaveUserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveUserViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("RegisterDeveloper", vm);

            var origin = Request.Headers["origin"];

            RegisterResponse response = await _developerService.Add(vm, origin);


            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View("RegisterDeveloper", vm);
            }

            return RedirectToRoute(new { controller = "Developer", action = "Index" });
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(int id)
        {
            DeveloperViewModel developer = await _developerService.GetByIdViewModel(id);
            SaveUserViewModel vm = new()
            {
                Id = developer.UserId,
                FirstName = developer.FirstName,
                LastName = developer.LastName,
                UserName = developer.Username,
                Email = developer.Email,
                Identification = developer.Identification,
                Role = (int)Roles.Developer,
            };

            return View("UpdateDeveloper", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveUserViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("UpdateDeveloper", vm);

            var response = await _developerService.Update(vm);

            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View("UpdateDeveloper", vm);
            }

            return RedirectToRoute(new { controller = "Developer", action = "Index" });
        }
        #endregion

        #region Active & Unactive User
        [HttpPost]
        public async Task<IActionResult> UpdateUserStatus(string username, string controller)
        {
            var response = await _developerService.UpdateUserStatus(username);

            if (response.HasError)
            {
                return RedirectToRoute(new { controller = "Developer", action = "Index", hasError = response.HasError, message = response.Error });
            }

            return RedirectToRoute(new { controller = "Developer", action = "Index", });
        }
        #endregion
    }
}
