using AutoMapper;
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

        public AdminService(IAdminRepository adminRepository, IMapper mapper) : base(adminRepository, mapper)
        {
            _mapper = mapper;
            _adminRepository = adminRepository;
        }
    }
}
