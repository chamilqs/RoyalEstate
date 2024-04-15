using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.ViewModels.Admins;
using RoyalState.Core.Application.ViewModels.Users;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Interfaces.Services
{
    public interface IAdminService : IGenericService<SaveAdminViewModel, AdminViewModel, Admin>
    {
        Task<RegisterResponse> Add(SaveUserViewModel vm, string origin);
        Task<UpdateUserResponse> Update(SaveUserViewModel vm);
        Task<GenericResponse> UpdateUserStatus(string username);
        Task<DahsboardViewModel> Dashboard();
    }
}
