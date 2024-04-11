using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.Agent;
using RoyalState.Core.Application.ViewModels.Improvements;
using RoyalState.Core.Application.ViewModels.Property;
using RoyalState.Core.Application.ViewModels.Users;

namespace RoyalState.Presentation.WebApp.Controllers
{
    public class AgentController : Controller
    {
        private readonly IAgentService _agentService;
        private readonly IImprovementService _improvmentService;
        private readonly ISaleTypeService _saleTypeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse authViewModel;

        public AgentController(IAgentService agentService, IHttpContextAccessor httpContextAccessor, IImprovementService improvmentService, ISaleTypeService saleTypeService)
        {
            _agentService = agentService;
            _httpContextAccessor = httpContextAccessor;
            authViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _improvmentService = improvmentService;
            _saleTypeService = saleTypeService;
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
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Please check for missing or incorrect fields";
                return View("EditProfile", vm);
            }

            if(vm.File != null)
            {
                vm.ImageUrl = await UploadFileAsync(vm.File, authViewModel.Email, true, vm.ImageUrl);
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

        #region Dashboard
        public IActionResult Dashboard()
        {

            return View();
        }

        public async Task<IActionResult> NewProperty()
        {
            ImprovementViewModel improvements = new();
            improvements.Id = 1;
            improvements.Name = "Swimming Pool";
            improvements.Description = "A nice swimming pool for the hot summer days.";      

            ImprovementViewModel improvements2 = new();
            improvements2.Id = 2;
            improvements2.Name = "Garden";
            improvements2.Description = "A beautiful garden to relax and enjoy the view.";

            List<ImprovementViewModel> improvementsList = new() { improvements, improvements2 };         
            ViewBag.Improvements = improvementsList;

            // ViewBag.SaleTypes = await _saleTypeService.GetAllViewModel();
            // ViewBag.Improvements = await _improvmentService.GetAllViewModel();

            return View(new SavePropertyViewModel());
        }

        [HttpPost]
        public IActionResult NewPropertyPost(SavePropertyViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Please check for missing or incorrect fields";
                return View("NewProperty", vm);
            }






            return RedirectToAction("NewProperty");
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
    }
}
