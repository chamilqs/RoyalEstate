using Swashbuckle.AspNetCore.Annotations;

namespace RoyalState.Core.Application.DTOs.Account
{
    /// <summary>
    /// Parameters for user modification
    /// </summary> 
    public class UpdateUserRequest
    {
        [SwaggerParameter(Description = " Id of the user")]
        public string Id { get; set; }
        [SwaggerParameter(Description = "First name of the user")]
        public string FirstName { get; set; }
        [SwaggerParameter(Description = "Last name of the user")]
        public string LastName { get; set; }

        [SwaggerParameter(Description = "The email of the user")]
        public string Email { get; set; }

        [SwaggerParameter(Description = "The username ")]
        public string UserName { get; set; }

        [SwaggerParameter(Description = "The password of the user")]
        public string Password { get; set; }

        [SwaggerParameter(Description = "The confirmation of the users password")]
        public string ConfirmPassword { get; set; }

        [SwaggerParameter(Description = "The phone number of the user")]
        public string Phone { get; set; }

        [SwaggerParameter(Description = "The profile picture of the user")]
        public string? ImageUrl { get; set; }

        [SwaggerParameter(Description = "The identification of the user")]
        public string? Identification { get; set; }
    }
}
