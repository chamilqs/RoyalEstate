using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace RoyalState.Core.Application.ViewModels.Users
{
    public class SaveUserViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "You must enter a name.")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "You must enter a lastname.")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "You mus enter a Username.")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "You must enter an email.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "The password must match")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "You must select a role.")]
        public int Role { get; set; }

        [RegularExpression(@"^\d{3}\-\d{7}-\d{1}$", ErrorMessage = "Your Id must be with the following format: ###-#######-#.")]
        [DataType(DataType.Text)]
        public string? Identification { get; set; }

        [DataType(DataType.Url)]
        public string? ImageUrl { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? File { get; set; }

        [DataType(DataType.Text)]
        public string? Phone { get; set; }

        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
