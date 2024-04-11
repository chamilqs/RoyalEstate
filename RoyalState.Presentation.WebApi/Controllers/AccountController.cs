using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Enums;
using RoyalState.Core.Application.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RoyalState.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Membership System")]

    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("authenticate")]
        [SwaggerOperation(
         Summary = "User LogIn",
         Description = "Authenticates an user and returns a JWT token."
     )]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            return Ok(await _accountService.AuthenticateAsync(request));
        }

        [HttpPost("developerRegister")]
        [SwaggerOperation(
             Summary = "Developer User registration",
             Description = "Recieves the necessary parameters for creating a developer user"
         )]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> RegisterDevAsync(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            request.Role = (int)Roles.Developer;
            return Ok(await _accountService.RegisterUserAsync(request, origin));
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("adminRegister")]
        [SwaggerOperation(
              Summary = "Admin User registration",
              Description = "Recieves the necessary parameters for creating an admin user"
          )]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> RegisterAdminAsync(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            request.Role = (int)Roles.Admin;
            return Ok(await _accountService.RegisterUserAsync(request, origin));
        }
    }
}
