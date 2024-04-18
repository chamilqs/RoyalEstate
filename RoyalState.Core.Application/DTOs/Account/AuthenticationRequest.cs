using Swashbuckle.AspNetCore.Annotations;

namespace RoyalState.Core.Application.DTOs.Account
{
    /// <summary>
    /// Parameters for user authentication
    /// </summary> 
    public class AuthenticationRequest
    {
        [SwaggerParameter(Description = "Credential of the user to be logged in ")]

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Credential { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [SwaggerParameter(Description = "Password of the user to be logged in ")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Password { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
