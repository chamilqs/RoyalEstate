using AutoMapper;
using Humanizer;
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
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpPost("authenticate")]
        [SwaggerOperation(
         Summary = "User LogIn",
         Description = "Authenticates an user and returns a JWT token."
     )]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            return Ok(await _accountService.AuthenticateWebApiAsync(request));
        }

        [HttpPost("developerRegister")]
        [SwaggerOperation(
             Summary = "Developer User registration",
             Description = "Recieves the necessary parameters for creating a developer user"
         )]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> RegisterDevAsync(RegisterDTO dto)
        {
            var origin = Request.Headers["origin"];
            var request = _mapper.Map<RegisterRequest>(dto);
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
        public async Task<IActionResult> RegisterAdminAsync(RegisterDTO dto)
        {
            var origin = Request.Headers["origin"];
            var request = _mapper.Map<RegisterRequest>(dto);
            request.Role = (int)Roles.Admin;
            return Ok(await _accountService.RegisterUserAsync(request, origin));
        }
    }
}
