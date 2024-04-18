using AutoMapper;
using MediatR;
using RoyalState.Core.Application.Exceptions;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RoyalState.Core.Application.Features.Agents.Commands.ChangeAgentStatus
{
    /// <summary>
    /// Parameters for changing the status of an agent 
    /// </summary> 

    public class ChangeAgentStatusCommand : IRequest<Response<int>>
    {
        [SwaggerParameter(Description = "Id of the agent to be changed ")]
        public int Id { get; set; }

        [SwaggerParameter(Description = "The status to be assigned to the agent. True / False")]
        public bool Status { get; set; }
    }
    public class ChangeAgentStatusCommandHandler : IRequestHandler<ChangeAgentStatusCommand, Response<int>>
    {
        private readonly IAgentRepository _agentRepository;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public ChangeAgentStatusCommandHandler(IAgentRepository agentRepository, IAccountService accountService, IMapper mapper)
        {
            _agentRepository = agentRepository;
            _mapper = mapper;
            _accountService = accountService;
        }

        public async Task<Response<int>> Handle(ChangeAgentStatusCommand command, CancellationToken cancellationToken)
        {
            var agent = await _agentRepository.GetByIdAsync(command.Id);
            if (agent == null) throw new ApiException($"Agent not found.", (int)HttpStatusCode.NotFound);
            await _accountService.ChangeUserStatus(agent.UserId, command.Status);
            return new Response<int>(command.Id);
        }
    }
}
