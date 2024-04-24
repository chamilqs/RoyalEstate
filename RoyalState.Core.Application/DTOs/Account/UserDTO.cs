namespace RoyalState.Core.Application.DTOs.Account
{
    public class UserDTO
    {

        public string Id { get; set; }


        public string UserName { get; set; }


        public string FirstName { get; set; }


        public string LastName { get; set; }


        public string Email { get; set; }

        public string? Phone { get; set; }

        public string Role { get; set; }

        public bool EmailConfirmed { get; set; }
    }
}
