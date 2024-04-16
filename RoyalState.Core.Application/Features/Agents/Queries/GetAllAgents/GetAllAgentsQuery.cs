using AutoMapper;
using MediatR;
using RoyalState.Core.Application.DTOs.Agent;
using RoyalState.Core.Application.Exceptions;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.Agent;
using RoyalState.Core.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
            if (agents == null) throw new ApiException($"Agents not found", (int)HttpStatusCode.NotFound);
            return new Response<IList<AgentDTO>>(agents);
        }

        private async Task<List<AgentDTO>> GetAllAgents()
        {
            var agentList = await _agentRepository.GetAllWithIncludeAsync(new List<string> { "Properties" });

            if (agentList == null || agentList.Count == 0) throw new ApiException($"Agents not found."
               , (int)HttpStatusCode.NotFound);

            var agentDtos = new List<AgentDTO>();

            foreach (var agent in agentList)
            {
                var agentUser = await _accountService.FindByIdAsync(agent.UserId);
                var agentDTO = new AgentDTO
                {
                    Id = agent.Id,
                    FirstName = agentUser.FirstName,
                    LastName = agentUser.LastName,
                    Email = agentUser.Email,
                    Phone = agentUser.Phone,
                    NumberOfProperties = agent.Properties.Count()

                };

                agentDtos.Add(agentDTO);

            }

            return agentDtos;
        }
    }
}
