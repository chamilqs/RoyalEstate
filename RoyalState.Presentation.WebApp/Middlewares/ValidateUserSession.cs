using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Helpers;

namespace RoyalState.Presentation.WebApp.Middlewares
{
    public class ValidateUserSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ValidateUserSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool HasUser()
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            AuthenticationResponse userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            if (userViewModel == null)
            {
                return false;
            }
            return true;
        }

    }
}
