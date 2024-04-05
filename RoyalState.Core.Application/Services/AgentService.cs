using AutoMapper;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.Users;
using RoyalState.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using RoyalState.Core.Application.ViewModels.Agent;

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

        #region GetByUserIdViewModel
        public async Task<AgentViewModel> GetByUserIdViewModel(string userId)
        {
            var clientList = await base.GetAllViewModel();
            AgentViewModel client = clientList.FirstOrDefault(client => client.UserId == userId);

            return client;
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


    }
}
