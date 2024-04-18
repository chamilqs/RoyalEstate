using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RoyalState.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {

        private IMediator _mediator;


#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8601 // Possible null reference assignment.
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore CS8603 // Possible null reference return.
    }
}
