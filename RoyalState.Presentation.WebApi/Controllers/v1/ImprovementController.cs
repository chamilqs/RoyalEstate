using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.DTOs.Agent;
using RoyalState.Core.Application.DTOs.TypeDTO;
using RoyalState.Core.Application.Features.Agents.Queries.GetAgentById;
using RoyalState.Core.Application.Features.Agents.Queries.GetAllAgents;
using RoyalState.Core.Application.Features.Improvements.Commands.CreateImprovement;
using RoyalState.Core.Application.Features.Improvements.Commands.DeleteImprovementById;
using RoyalState.Core.Application.Features.Improvements.Commands.UpdateImprovement;
using RoyalState.Core.Application.Features.Improvements.Queries.GetAllImprovements;
using RoyalState.Core.Application.Features.Improvements.Queries.GetImprovementById;
using RoyalState.WebApi.Controllers.v1;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RoyalState.Presentation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Developer,Admin")]
    [SwaggerTag("Improvement management")]
    public class ImprovementController : BaseApiController
    {

        [HttpGet]
        [SwaggerOperation(
           Summary = "Improvement list",
           Description = "Returns a list of all the improvements"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TypeDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetAllImprovementsQuery()));
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
           Summary = "Improvement by id",
           Description = "Returns an improvement using the id as a filter"
       )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TypeDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetImprovementByIdQuery { Id = id }));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [SwaggerOperation(
         Summary = "Improvement creation",
         Description = "Recieves the parameters for creating an improvement"
       )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(CreateImprovementCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(await Mediator.Send(command));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [SwaggerOperation(
             Summary = "Improvement update",
             Description = "Recieves the parameters for modifying an improvement"
      )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImprovementUpdateResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, UpdateImprovementCommand command)
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

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Improvement removal",
            Description = "Recieves parameters for eliminating an Improvement"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteImprovementByIdCommand { Id = id });
            return NoContent();
        }

    }
}
