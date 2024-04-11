using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using RoyalState.Core.Application.DTOs.Agent;
using RoyalState.Core.Application.DTOs.Property;
using RoyalState.Core.Application.Features.Agents.Commands.ChangeAgentStatus;
using RoyalState.Core.Application.Features.Agents.Queries.GetAgentById;
using RoyalState.Core.Application.Features.Agents.Queries.GetAgentPropertyById;
using RoyalState.Core.Application.Features.Agents.Queries.GetAllAgents;
using RoyalState.WebApi.Controllers.v1;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RoyalState.Presentation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Developer,Admin")]
    [SwaggerTag("Mantenimiento de productos")]

    public class AgentController : BaseApiController
    {
        [HttpGet]
        [SwaggerOperation(
           Summary = "Agents List",
           Description = "Returns a list of all the agents"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgentDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetAllAgentsQuery()));
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
           Summary = "Agent by id",
           Description = "Returns an agent using the id as a filter"
       )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgentDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetAgentByIdQuery { Id = id }));
        }


        [HttpGet("{agentId}")]
        [SwaggerOperation(
          Summary = "Property by agent id",
          Description = "Returns a property using the agent id as a filter"
      )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProperties(int agentId)
        {
            return Ok(await Mediator.Send(new GetAgentPropertyByIdQuery { AgentId = agentId }));
        }

        [HttpPatch("{id}")]
        [SwaggerOperation(
               Summary = "Status Change of an agent",
               Description = "Accepts the parameters to change the status of an agent."
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgentDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeStatus(int id, ChangeAgentStatusCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (id != command.Id)
            {
                return BadRequest();
            }

            return Ok(await Mediator.Send(command));
        }


    }
}
