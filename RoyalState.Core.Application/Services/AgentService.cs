using AutoMapper;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.Users;
using RoyalState.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using RoyalState.Core.Application.ViewModels.Agent;
using RoyalState.Core.Application.ViewModels.User;
using RoyalState.Core.Application.ViewModels.Client;

namespace RoyalState.Core.Application.Services
{
    public class AgentService : GenericService<SaveAgentViewModel, AgentViewModel, Agent>, IAgentService
    {
        private readonly IAgentRepository _agentRepository;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse user;
        private readonly IMapper _mapper;

        public AgentService(IAgentRepository agentRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper, IUserService userService) : base(agentRepository, mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _agentRepository = agentRepository;
            _mapper = mapper;
            user = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _userService = userService;
        }

        #region Update
        public async Task<UpdateUserResponse> UpdateAsync(SaveUserViewModel vm)
        {
            UpdateUserResponse response = await _userService.UpdateUserAsync(vm);

            if (!response.HasError)
            {
                var agent = await GetByUserIdViewModel(vm.Id);
                if (vm.ImageUrl == null)
                {
                    vm.ImageUrl = agent.ImageUrl;
                }

                Agent avm = new()
                {
                    Id = agent.Id,
                    UserId = agent.UserId,
                    ImageUrl = vm.ImageUrl,
                    CreatedBy = user.UserName,
                    CreatedDate = DateTime.UtcNow,
                    LastModifiedBy = user.UserName,
                    LastModifiedDate = DateTime.UtcNow
                };

                await _agentRepository.UpdateAsync(avm, agent.Id);

            }
            else
            {
                response.HasError = true;
                response.Error = response.Error;
                return response;
            }

            return response;
        } 
        #endregion

        #region Register
        public async Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin)
        {
            RegisterResponse response = await _userService.RegisterAsync(vm, origin);

            if (!response.HasError)
            {
                var user = await _userService.GetByEmailAsync(vm.Email);

                SaveAgentViewModel saveAgentViewModel = new()
                {
                    UserId = user.Id,
                    ImageUrl = vm.ImageUrl,
                };

                await base.Add(saveAgentViewModel);

            }

            return response;
        }
        #endregion

        #region Get Methods

        #region GetAll Overriden
        public override async Task<List<AgentViewModel>> GetAllViewModel()
        {
            var agents = await base.GetAllViewModel();
            var agentsViewModels = new List<AgentViewModel>();

            foreach (var agent in agents)
            {
                var user = await _userService.GetByIdAsync(agent.UserId);

                if (user.EmailConfirmed)
                {
                    var agentViewModel = new AgentViewModel
                    {
                        Id = agent.Id,
                        UserId = agent.UserId,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        EmailConfirmed = user.EmailConfirmed,
                        Phone = user.Phone,
                        ImageUrl = agent.ImageUrl,
                    };

                    agentsViewModels.Add(agentViewModel);
                }

            }

            return agentsViewModels;

        }
        #endregion

        #region GetByNameViewModel
        public async Task<List<AgentViewModel>> GetByNameViewModel(string name)
        {
            List<UserViewModel> users = await _userService.GetByNameAsync(name);

            if (users == null)
            {
                return null;
            }

            var agentsViewModels = new List<AgentViewModel>();

            foreach (var user in users)
            {
                var agent = await GetByUserIdViewModel(user.Id);

                AgentViewModel vm = new()
                {
                    Id = agent.Id,
                    UserId = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Phone = user.Phone,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    ImageUrl = agent.ImageUrl
                };

                agentsViewModels.Add(vm);
            }

            return agentsViewModels;
        }
        #endregion

        #region GetByUserIdViewModel
        public async Task<AgentViewModel> GetByUserIdViewModel(string userId)
        {
            var agentList = await base.GetAllViewModel();
            AgentViewModel agent = agentList.FirstOrDefault(agent => agent.UserId == userId);

            return agent;
        }
        public async Task<SaveUserViewModel> GetProfileDetails()
        {
            var agent = await GetByUserIdViewModel(user.Id);

            SaveUserViewModel vm = await _userService.GetUserSaveViewModel(user.Id);

            if (agent != null)
            {
                vm.ImageUrl = agent.ImageUrl;
            }

            return vm;
        }
        #endregion

        #endregion
    }
}

