using AutoMapper;
using MediatR;
using RoyalState.Core.Application.DTOs.Agent;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.Wrappers;

namespace RoyalState.Core.Application.Features.Agents.Queries.GetAllAgents
{
    public class GetAllAgentsQuery : IRequest<Response<IList<AgentDTO>>>
    {
    }

    public class GetAllAgentsQueryHandler : IRequestHandler<GetAllAgentsQuery, Response<IList<AgentDTO>>>
    {
        private readonly IAgentRepository _agentRepository;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public GetAllAgentsQueryHandler(IAgentRepository agentRepository, IAccountService accountService, IMapper mapper)
        {
            _agentRepository = agentRepository;
            _mapper = mapper;
            _accountService = accountService;
        }

        public async Task<Response<IList<AgentDTO>>> Handle(GetAllAgentsQuery request, CancellationToken cancellationToken)
        {
            var agents = await GetAllAgents();
            if (agents == null) return new Response<IList<AgentDTO>>("Agents not found");
            return new Response<IList<AgentDTO>>(agents);
        }

        private async Task<List<AgentDTO>> GetAllAgents()
        {
            var agentList = await _agentRepository.GetAllWithIncludeAsync(new List<string> { "Properties" });

#pragma warning disable CS8603 // Possible null reference return.
            if (agentList == null || agentList.Count == 0) return null;
#pragma warning restore CS8603 // Possible null reference return.

            var agentDtos = new List<AgentDTO>();

            foreach (var agent in agentList)
            {
                var agentUser = await _accountService.FindByIdAsync(agent.UserId);
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8601 // Possible null reference assignment.
                var agentDTO = new AgentDTO
                {
                    Id = agent.Id,
                    FirstName = agentUser.FirstName,
                    LastName = agentUser.LastName,
                    Email = agentUser.Email,
                    Phone = agentUser.Phone,
                    NumberOfProperties = agent.Properties.Count()

                };
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore CS8604 // Possible null reference argument.

                agentDtos.Add(agentDTO);

            }

            return agentDtos;
        }
    }
}
