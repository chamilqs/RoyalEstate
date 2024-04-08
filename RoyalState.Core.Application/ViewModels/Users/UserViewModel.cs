namespace RoyalState.Core.Application.ViewModels.User
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Identification { get; set; }
        public string? Phone { get; set; }
        public string Role { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
