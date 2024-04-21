using Swashbuckle.AspNetCore.Annotations;

namespace RoyalState.Core.Application.DTOs.Account
{
    /// <summary>
    /// Parameters for user authentication
    /// </summary> 
    public class AuthenticationRequest
    {
        [SwaggerParameter(Description = "Credential of the user to be logged in ")]


        public string Credential { get; set; }


        [SwaggerParameter(Description = "Password of the user to be logged in ")]

        public string Password { get; set; }

    }
}
