﻿namespace RoyalState.Core.Application.ViewModels.Agent
{
    public class AgentViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Phone { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
