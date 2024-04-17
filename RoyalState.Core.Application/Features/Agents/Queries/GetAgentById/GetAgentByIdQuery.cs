using AutoMapper;
using MediatR;
using RoyalState.Core.Application.DTOs.Agent;
using RoyalState.Core.Application.Exceptions;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.Agent;
using RoyalState.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RoyalState.Core.Application.Features.Agents.Queries.GetAgentById
{
    /// <summary>
    /// Parameters to filter agents by id 
    /// </summary>  
    public class GetAgentByIdQuery : IRequest<Response<AgentDTO>>
    {
        [SwaggerParameter(Description = "Insert the id of the agent to obtain. ")]
        [Required]
        public int Id { get; set; }
    }
    public class GetAgentByIdQueryHandler : IRequestHandler<GetAgentByIdQuery, Response<AgentDTO>>
    {
        private readonly IAgentRepository _agentRepository;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public GetAgentByIdQueryHandler(IAgentRepository agentRepository, IAccountService accountService, IMapper mapper)
        {
            _agentRepository = agentRepository;
            _mapper = mapper;
            _accountService = accountService;

        }

        public async Task<Response<AgentDTO>> Handle(GetAgentByIdQuery request, CancellationToken cancellationToken)
        {
            var agent = await GetByIdViewModel(request.Id);
            if (agent == null) throw new ApiException($"Agent not found.", (int)HttpStatusCode.NoContent);
            return new Response<AgentDTO>(agent);
        }
        private async Task<AgentDTO> GetByIdViewModel(int id)
        {
            var agentList = await _agentRepository.GetAllWithIncludeAsync(new List<string> { "Properties" });
            if (agentList == null) throw new ApiException($"Agents not found.", (int)HttpStatusCode.NoContent);
            var agent = agentList.Where(a => a.Id == id).FirstOrDefault();
            var agentUser = await _accountService.FindByIdAsync(agent.UserId);
            AgentDTO agentDTO = new()
            {
                Id = id,
                FirstName = agentUser.FirstName,
                LastName = agentUser.LastName,
                Email = agentUser.Email,
                NumberOfProperties = agent.Properties.Count(),
                Phone = agentUser.Phone

            };
            return agentDTO;
        }

    }
}
