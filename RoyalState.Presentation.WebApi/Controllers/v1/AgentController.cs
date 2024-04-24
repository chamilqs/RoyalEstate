using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.DTOs.Agent;
using RoyalState.Core.Application.DTOs.Property;
using RoyalState.Core.Application.Exceptions;
using RoyalState.Core.Application.Features.Agents.Commands.ChangeAgentStatus;
using RoyalState.Core.Application.Features.Agents.Queries.GetAgentById;
using RoyalState.Core.Application.Features.Agents.Queries.GetAgentPropertyById;
using RoyalState.Core.Application.Features.Agents.Queries.GetAllAgents;
using RoyalState.WebApi.Controllers.v1;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;

namespace RoyalState.Presentation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Developer,Admin")]
    [SwaggerTag("Agent management")]

    public class AgentController : BaseApiController
    {
        [HttpGet]
        [SwaggerOperation(
           Summary = "Agents List",
           Description = "Returns a list of all the agents"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgentDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {


            var agents = await Mediator.Send(new GetAllAgentsQuery());

            if (agents.Data == null || agents.Data.Count == 0)
            {
                return NoContent();
            }

            if (!agents.Succeeded)
            {
                throw new ApiException("Server Error", (int)HttpStatusCode.InternalServerError);
            }

            return Ok(agents);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
           Summary = "Agent by id",
           Description = "Returns an agent using the id as a filter"
       )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgentDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {

            var agent = await Mediator.Send(new GetAgentByIdQuery { Id = id });

            if (agent.Data == null)
            {
                return NoContent();
            }

            if (!agent.Succeeded)
            {
                throw new ApiException("Server Error", (int)HttpStatusCode.InternalServerError);
            }

            return Ok(agent);
        }


        [HttpGet("{agentId}/properties")]
        [SwaggerOperation(
          Summary = "Property by agent id",
          Description = "Returns a property using the agent id as a filter"
      )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAgentProperty(int agentId)
        {
            var properties = await Mediator.Send(new GetAgentPropertyByIdQuery { AgentId = agentId });

            if (properties.Data == null || properties.Data.Count == 0)
            {
                return NoContent();
            }

            if (!properties.Succeeded)
            {
                throw new ApiException("Server Error", (int)HttpStatusCode.InternalServerError);
            }

            return Ok(properties);

        }

        [Authorize(Roles = "Admin")]
        [HttpPatch]
        [SwaggerOperation(
               Summary = "Status Change of an agent",
               Description = "Accepts the parameters to change the status of an agent."
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeStatus(ChangeAgentStatusCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }


            await Mediator.Send(command);
            return NoContent();

        }


    }
}
