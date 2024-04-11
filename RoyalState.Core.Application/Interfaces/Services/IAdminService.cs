using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.ViewModels.Admins;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Interfaces.Services
{
    public interface IAdminService : IGenericService<SaveAdminViewModel, AdminViewModel, Admin>
    {
        Task<GenericResponse> UpdateUserStatus(string userId);
    }
}
