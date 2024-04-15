using AutoMapper;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.Admins;
using RoyalState.Core.Application.ViewModels.Users;
using RoyalState.Core.Domain.Entities;
using System.Collections.Generic;

namespace RoyalState.Core.Application.Services
{
    public class AdminService : GenericService<SaveAdminViewModel, AdminViewModel, Admin>, IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IAgentService _agentService;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly IPropertyService _propertyService;

        public AdminService(IAdminRepository adminRepository, IMapper mapper, IAccountService accountService, IUserService userService, IPropertyService propertyService, IAgentService agentService) : base(adminRepository, mapper)
        {
            _mapper = mapper;
            _adminRepository = adminRepository;
            _accountService = accountService;
            _userService = userService;
            _propertyService = propertyService;
            _agentService = agentService;
        }

        #region Dashboard
        public async Task<DahsboardViewModel> Dashboard()
        {
            DahsboardViewModel dashboard = new();
            var agents = await _accountService.GetAllAgentAsync();
            var clients = await _accountService.GetAllClientAsync();
            var developers = await _accountService.GetAllDeveloperAsync();
            var properties = await _propertyService.GetAllViewModel();

            dashboard.ActiveAgents = agents.FindAll(agent => agent.EmailConfirmed == true).Count;
            dashboard.UnactiveAgents = agents.FindAll(agent => agent.EmailConfirmed == false).Count;

            dashboard.ActiveDevelopers = developers.FindAll(dev => dev.EmailConfirmed == true).Count;
            dashboard.UnactiveDevelopers = developers.FindAll(dev => dev.EmailConfirmed == false).Count;

            dashboard.ActiveClients = clients.FindAll(client => client.EmailConfirmed == true).Count;
            dashboard.UnactiveClients = clients.FindAll(client => client.EmailConfirmed == false).Count;

            dashboard.PropertyQuantity = properties.Count;
                
            return dashboard;
        }
        #endregion

        #region Add
        public async Task<RegisterResponse> Add(SaveUserViewModel vm, string origin)
        {
            RegisterResponse response = await _userService.RegisterAsync(vm, origin);

            if (!response.HasError)
            {

                var user = await _userService.GetByEmailAsync(vm.Email);
                var activeUser = await UpdateUserStatus(user.UserName);
                
                if (!activeUser.HasError)
                {
                    SaveAdminViewModel saveAdminViewModel = new()
                    {
                        UserId = user.Id,
                        Identification = vm.Identification,
                        CreatedBy = "DefaultAppUser",
                        CreatedDate = DateTime.Now
                    };

                    await base.Add(saveAdminViewModel);
                }
                else
                {
                    response.HasError = activeUser.HasError;
                    response.Error = activeUser.Error;
                }
            }

            return response;
        }
        #endregion

        #region Update
        public async Task<UpdateUserResponse> Update(SaveUserViewModel vm)
        {
            UpdateUserResponse response = await _userService.UpdateUserAsync(vm);

            if (!response.HasError)
            {
                var admiList = await base.GetAllViewModel();
                var admin = admiList.Find(admin => admin.UserId == vm.Id);

                SaveAdminViewModel saveAdminViewModel = await base.GetByIdSaveViewModel(admin.Id);
                saveAdminViewModel.Identification = vm.Identification;

                await base.Update(saveAdminViewModel, admin.Id);
            }

            return response;
        }
        #endregion

        #region GetAll
        public override async Task<List<AdminViewModel>> GetAllViewModel()
        {
            List<AdminViewModel> adminList = await base.GetAllViewModel();
            List<UserDTO> userAdminList = await _accountService.GetAllAdminAsync();

            foreach (var admin in adminList)
            {
                var user = userAdminList.Find(user => user.Id == admin.UserId);

                admin.FirstName = user.FirstName;
                admin.LastName = user.LastName;
                admin.Username = user.UserName;
                admin.Email = user.Email;
                admin.EmailConfirmed = user.EmailConfirmed;
            }

            return adminList;
        }
        #endregion

        #region GetByIdViewModel
        public override async Task<AdminViewModel> GetByIdViewModel(int id)
        {
            List<AdminViewModel> adminList = await GetAllViewModel();

            return adminList.Find(admin => admin.Id == id);
        }
        #endregion

        #region DeleteAgent
        public async Task<GenericResponse> DeleteAgent(int id)
        {
            var agent = await _agentService.GetByIdViewModel(id);
            GenericResponse response = new();

            if (agent != null)
            {
                response = await _propertyService.DeleteByAgentId(agent.Id);

                if (!response.HasError)
                {
                    await _agentService.Delete(id);
                    response = await _accountService.DeleteUserAsync(agent.UserId);
                }
                else
                {
                    response.HasError = true;
                    response.Error = response.Error;
                }
            }

            return response;
        }
        #endregion

        #region UpdateUserStatus
        public async Task<GenericResponse> UpdateUserStatus(string username)
        {
            var userStatus = await _accountService.UpdateUserStatusAsync(username);

            return userStatus;
        }
        #endregion
    }
}
