using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.Agent;
using RoyalState.Core.Application.ViewModels.Users;

namespace RoyalState.Presentation.WebApp.Controllers
{
    public class AgentController : Controller
    {
        private readonly IAgentService _agentService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse authViewModel;

        public AgentController(IAgentService agentService, IHttpContextAccessor httpContextAccessor)
        {
            _agentService = agentService;
            _httpContextAccessor = httpContextAccessor;
            authViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        #region Get
        public async Task<IActionResult> AllAgents()
        {
            var agents = await _agentService.GetAllViewModel();
            return View(agents);
        } 
        #endregion

        #region Edit
        public async Task<IActionResult> EditProfile()
        {
            SaveUserViewModel vm = await _agentService.GetProfileDetails();
            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> EditProfile(SaveUserViewModel vm)
        {
            if(vm.File != null)
            {
                vm.ImageUrl = await UploadFileAsync(vm.File, authViewModel.Email, true, vm.ImageUrl);
            }

            if (!ModelState.IsValid)
            {
                return View("EditProfile", vm);
            }

            UpdateUserResponse response = new();
            response = await _agentService.UpdateAsync(vm);

            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View("EditProfile", vm);
            }

            return RedirectToAction("EditProfile");
        }
        #endregion

        #region Private Methods
        private async Task<string> UploadFileAsync(IFormFile file, string email, bool isEditMode = false, string imagePath = "")
        {
            if (isEditMode && file == null)
            {
                return imagePath;
            }

            string basePath = $"/Images/Users/{email}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            Guid guid = Guid.NewGuid();
            string fileName = $"{guid}{Path.GetExtension(file.FileName)}";
            string fileNameWithPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            if (isEditMode)
            {
                string[] oldImagePart = imagePath.Split("/");
                string oldImagePath = oldImagePart[^1];
                string completeImageOldPath = Path.Combine(path, oldImagePath);

                if (System.IO.File.Exists(completeImageOldPath))
                {
                    System.IO.File.Delete(completeImageOldPath);
                }
            }

            return $"{basePath}/{fileName}";
        }

        #endregion

        #region Search
        public async Task<IActionResult> SearchAgent(string agentName)
        {
            var agents = await _agentService.GetByNameViewModel(agentName);
            bool isEmpty = agents == null;

            ViewBag.IsEmpty = isEmpty;
            return View("AllAgents", agents != null ? agents : new List<AgentViewModel>());
        }
        #endregion

    }
}
