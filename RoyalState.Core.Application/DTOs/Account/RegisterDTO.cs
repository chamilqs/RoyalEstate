using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalState.Core.Application.DTOs.Account
{
    /// <summary>
    /// Parameters for developer and admin registration
    /// </summary> 
    public class RegisterDTO
    {
        [SwaggerParameter(Description = "First name of the user")]
        public string FirstName { get; set; }

        [SwaggerParameter(Description = "Last name of the user")]
        public string LastName { get; set; }

        [SwaggerParameter(Description = "The email of the user")]
        public string Email { get; set; }

        [SwaggerParameter(Description = "The username ")]
        public string UserName { get; set; }

        [SwaggerParameter(Description = "The identification of the user")]
        public string? Identification { get; set; }


        [SwaggerParameter(Description = "The password of the user")]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "The password must match")]
        [SwaggerParameter(Description = "The confirmation of the users password")]
        public string ConfirmPassword { get; set; }

    }
}
