using AutoMapper;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.Admins;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Services
{
    public class AdminService : GenericService<SaveAdminViewModel, AdminViewModel, Admin>, IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public AdminService(IAdminRepository adminRepository, IMapper mapper, IAccountService accountService) : base(adminRepository, mapper)
        {
            _mapper = mapper;
            _adminRepository = adminRepository;
            _accountService = accountService;
        }



        #region UpdateUserStatus
        public async Task<GenericResponse> UpdateUserStatus(string userId)
        {
            var userStatus = await _accountService.UpdateUserStatusAsync(userId);

            return userStatus;
        }
        #endregion
    }
}
