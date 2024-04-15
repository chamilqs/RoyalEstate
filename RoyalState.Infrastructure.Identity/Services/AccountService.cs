using Microsoft.AspNetCore.Identity;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Enums;
using RoyalState.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Domain.Settings;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using RoyalState.Core.Application.DTOs.Email;
using Microsoft.EntityFrameworkCore;
using Azure.Core;
using RoyalState.Core.Application.Wrappers;
using MediatR;

namespace RoyalState.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JWTSettings _jwtSettings;
        private readonly IEmailService _emailService;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<JWTSettings> jwtSettings, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
            _emailService = emailService;
        }

        #region Login

        #region AuthenticateApi
        public async Task<AuthenticationResponse> AuthenticateWebApiAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new();

            var user = await _userManager.FindByEmailAsync(request.Credential);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(request.Credential);
                if (user == null)
                {
                    response.HasError = true;
                    response.Error = $"No accounts registered with email or username: {request.Credential}";
                    return response;
                }

            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Invalid credentials, please try again.";
                return response;
            }
            if (!user.EmailConfirmed)
            {
                response.HasError = true;
                response.Error = $"Account not confirmed for {user.Email}";
                return response;
            }

            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);

            response.Id = user.Id;
            response.Email = user.Email;
            response.UserName = user.UserName;

            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            var refreshToken = GenerateRefreshToken();
            response.RefreshToken = refreshToken.Token;

            return response;
        }
        #endregion

        #region AuthenticateWebApp
        public async Task<AuthenticationResponse> AuthenticateWebAppAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new();

            var user = await _userManager.FindByEmailAsync(request.Credential);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(request.Credential);
                if (user == null)
                {
                    response.HasError = true;
                    response.Error = $"No accounts registered with email or username: {request.Credential}";
                    return response;
                }

            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Invalid credentials, please try again.";
                return response;
            }
            if (!user.EmailConfirmed)
            {
                response.HasError = true;
                response.Error = $"Account not confirmed for {user.Email}";
                return response;
            }

            response.Id = user.Id;
            response.Email = user.Email;
            response.UserName = user.UserName;

            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;


            return response;
        }
        #endregion

        #region SingOut
        public async Task SingOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        #endregion

        #endregion

        #region Register
        public async Task<RegisterResponse> RegisterUserAsync(RegisterRequest request, string origin)
        {
            RegisterResponse response = new()
            {
                HasError = false
            };

            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"Username '{request.UserName}' is already taken.";
                return response;
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Error = $"Email '{request.Email}'is already registered.";
                return response;
            }

            var user = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.Phone,
                Email = request.Email,
                UserName = request.UserName,
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                switch (request.Role)
                {
                    case (int)Roles.Client:

                        await _userManager.AddToRoleAsync(user, Roles.Client.ToString());
                        // Here we send an email to the user to confirm the account
                        var verificationUri = await SendVerificationEmailUri(user, origin);
                        await _emailService.SendAsync(new EmailRequest()
                        {
                            To = user.Email,
                            Subject = "Confirm your registration at RoyalState.",
                            Body = $"Please confirm your account by visiting this URL {verificationUri}"
                        });
                        break;

                    case (int)Roles.Agent:

                        await _userManager.AddToRoleAsync(user, Roles.Agent.ToString());

                        break;

                    case (int)Roles.Admin:

                        await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());

                        break;

                    case (int)Roles.Developer:

                        await _userManager.AddToRoleAsync(user, Roles.Developer.ToString());
                        break;

                    default:
                        response.HasError = true;
                        response.Error = $"An error has occurred trying to register the user.";
                        return response;
                }
            }
            else
            {
                response.HasError = true;
                response.Error = $"An error has occurred trying to register the user.";
                return response;
            }

            return response;
        }

        public async Task<string> ConfirmAccountAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return $"No accounts registered with this user.";
            }

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return $"Account confirmed for {user.Email}. You can now use the app.";
            }
            else
            {
                return $"An error occurred while trying to confirm the email: {user.Email}.";
            }
        }

        #endregion

        #region Update
        public async Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest request)
        {
            UpdateUserResponse response = new()
            {
                HasError = false
            };

            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null && userWithSameUserName.Id != request.Id)
            {
                response.HasError = true;
                response.Error = $"Username '{request.UserName}' is already taken.";
                return response;
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null && userWithSameEmail.Id != request.Id)
            {
                response.HasError = true;
                response.Error = $"Email '{request.Email}'is already registered.";
                return response;
            }

            var user = await _userManager.FindByIdAsync(request.Id);

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.UserName = request.UserName;
            user.PhoneNumber = request.Phone;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                if (request.Password != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    result = await _userManager.ResetPasswordAsync(user, token, request.Password);

                    if (!result.Succeeded)
                    {
                        response.HasError = true;
                        response.Error = $"An error ocurred while trying to update the password.";
                        return response;
                    }
                }
            }
            else
            {
                response.HasError = true;
                response.Error = $"An error ocurred while trying to update the user.";
                return response;
            }

            return response;
        }

        #endregion

        #region Active & Unactive 
        public async Task<GenericResponse> UpdateUserStatusAsync(string username)
        {
            GenericResponse response = new()
            {
                HasError = false
            };

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                response.HasError = true;
                response.Error = $"User: {username} not found.";
                return response;
            }

            user.EmailConfirmed = !user.EmailConfirmed;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"An error has ocurred trying to update the status of the user: {username}.";
                return response;
            }

            return response;
        }
        #endregion

        #region Finders

        public async Task<List<UserDTO>> FindByNameAsync(string name)
        {
            List<UserDTO> userDTOs = new List<UserDTO>();
            var users = await _userManager.Users.Where(u => (u.FirstName.Contains(name) || (u.FirstName + " " + u.LastName).Contains(name))).ToListAsync();

            foreach (var user in users)
            {
                UserDTO userDTO = new UserDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    EmailConfirmed = user.EmailConfirmed
                };

                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                if (rolesList.Any(r => r != Roles.Agent.ToString()))
                {
                    continue;
                }

                userDTO.Role = rolesList.ToList()[0];

                userDTOs.Add(userDTO);
            }

            return userDTOs;
        }

        public async Task<UserDTO> FindByEmailAsync(string email)
        {
            UserDTO userDTO = new();

            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                userDTO.Id = user.Id;
                userDTO.UserName = user.UserName;
                userDTO.FirstName = user.FirstName;
                userDTO.LastName = user.LastName;
                userDTO.Email = user.Email;
                userDTO.Phone = user.PhoneNumber;
                userDTO.EmailConfirmed = user.EmailConfirmed;

                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                userDTO.Role = rolesList.ToList()[0];

                return userDTO;
            }

            return null;
        }

        public async Task<UserDTO> FindByIdAsync(string Id)
        {
            UserDTO userDTO = new();

            var user = await _userManager.FindByIdAsync(Id);
            if (user != null)
            {
                userDTO.Id = user.Id;
                userDTO.UserName = user.UserName;
                userDTO.FirstName = user.FirstName;
                userDTO.LastName = user.LastName;
                userDTO.Email = user.Email;
                userDTO.Phone = user.PhoneNumber;
                userDTO.EmailConfirmed = user.EmailConfirmed;

                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                userDTO.Role = rolesList.ToList()[0];

                return userDTO;
            }

            return null;
        }

        #endregion

        #region GetAllUserAsync
        public async Task<List<UserDTO>> GetAllAdminAsync()
        {
            var userDTOList = await GetAllUserAsync();
            userDTOList = userDTOList.Where(user => user.Role == Roles.Admin.ToString()).ToList();

            return userDTOList;
        }

        public async Task<List<UserDTO>> GetAllDeveloperAsync()
        {
            var userDTOList = await GetAllUserAsync();
            userDTOList = userDTOList.Where(user => user.Role == Roles.Developer.ToString()).ToList();

            return userDTOList;
        }

        public async Task<List<UserDTO>> GetAllClientAsync()
        {
            var userDTOList = await GetAllUserAsync();
            userDTOList = userDTOList.Where(user => user.Role == Roles.Client.ToString()).ToList();

            return userDTOList;
        }

        public async Task<List<UserDTO>> GetAllAgentAsync()
        {
            var userDTOList = await GetAllUserAsync();
            userDTOList = userDTOList.Where(user => user.Role == Roles.Agent.ToString()).ToList();

            return userDTOList;
        }
        #endregion

        #region Private Methods

        #region JWT Methods
        private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        private RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow
            };
        }

        private string RandomTokenString()
        {
            var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);

            return BitConverter.ToString(randomBytes).Replace("-", "");
        }
        #endregion

        #region Email Methods

        private async Task<string> SendVerificationEmailUri(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "User/ConfirmEmail";
            var Uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "token", code);

            return verificationUri;
        }

        private async Task<string> SendForgotPasswordUri(ApplicationUser user, string origin)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "User/ResetPassword";
            var Uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "token", code);

            return verificationUri;
        }

        #endregion

        #region GetAllUserAsync
        private async Task<List<UserDTO>> GetAllUserAsync()
        {
            var userList = await _userManager.Users.ToListAsync();

            var userDTOList = userList.Select(user => new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed
            }).ToList();

            foreach (var userDTO in userDTOList)
            {
                var user = await _userManager.FindByIdAsync(userDTO.Id);

                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                userDTO.Role = rolesList.ToList()[0];
            }

            return userDTOList;
        }
        #endregion

        #endregion
    }
}
