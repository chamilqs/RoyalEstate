using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Enums;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.Users;
using RoyalState.Infrastructure.Identity.Entities;
using RoyalState.Presentation.WebApp.Middlewares;

namespace WebAdmin.BankingApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAgentService _agentService;
        private readonly IClientService _clientService;
        private readonly IUserService _userService;
        private readonly IFileService _fileService;
        private readonly AuthenticationResponse authViewModel;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IUserService userService, IAgentService agentService, IClientService clientService, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, IFileService fileService)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            authViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _agentService = agentService;
            _clientService = clientService;
            _fileService = fileService;
        }

        #region Login & Logout
        [ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> Index(bool hasError = false, string? message = null)
        {
            var login = new LoginViewModel();

            if (hasError)
            {
                login.HasError = hasError;
                login.Error = message;
            }

            return View(login);
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            AuthenticationResponse userVm = await _userService.LoginAsync(vm);
            if (userVm != null && !userVm.HasError)
            {
                HttpContext.Session.Set("user", userVm);
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            else
            {
                vm.HasError = userVm.HasError;
                vm.Error = userVm.Error;
                return View(vm);
            }
        }

        // LogOut
        public async Task<IActionResult> LogOut()
        {
            await _userService.SignOutAsync();
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
        #endregion

        #region Register
        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult Register()
        {
            return View(new SaveUserViewModel());
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> Register(SaveUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Error", "Please fill all the required fields");
                return View(vm);
            }

            vm.ImageUrl = await _fileService.UploadFileAsync(vm.File, vm.Email);
            var origin = Request.Headers["origin"];

            RegisterResponse response = vm.Role switch
            {
                (int)Roles.Agent => await _agentService.RegisterAsync(vm, origin),
                (int)Roles.Client => await _clientService.RegisterAsync(vm, origin),
                _ => new RegisterResponse()
            };

            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                ViewBag.Error = response.Error;
                return View(vm);
            }

            ViewBag.Email = vm.Email;
            if (vm.Role == (int)Roles.Client)
            {
                return RedirectToAction("SuccessRegistration", new { email = vm.Email, isClient = true });
            }
            else if (vm.Role == (int)Roles.Agent)
            {
                return RedirectToAction("SuccessRegistration", new { email = vm.Email, isClient = false });
            }

            return RedirectToRoute(new { controller = "User", action = "Index" });

        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult SuccessRegistration(string email, bool isClient)
        {
            ViewBag.Email = email;
            return View(isClient);
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            string response = await _userService.ConfirmEmailAsync(userId, token);
            return View("ConfirmEmail", response);
        }

        #endregion

        #region Authorization
        public async Task<IActionResult> RedirectIndex(string? ReturnUrl)
        {
            return RedirectToRoute(new { controller = "User", action = "Index", hasError = true, message = "You don't have access to this section!" });
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        #endregion
    }
}

