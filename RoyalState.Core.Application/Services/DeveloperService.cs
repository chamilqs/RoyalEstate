using AutoMapper;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.Developers;
using RoyalState.Core.Application.ViewModels.Users;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Services
{
    public class DeveloperService : GenericService<SaveDeveloperViewModel, DeveloperViewModel, Developer>, IDeveloperService
    {
        private readonly IDeveloperRepository _developerRepository;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly IPropertyService _propertyService;

        public DeveloperService(IDeveloperRepository developerRepository, IMapper mapper, IAccountService accountService, IUserService userService, IPropertyService propertyService) : base(developerRepository, mapper)
        {
            _mapper = mapper;
            _developerRepository = developerRepository;
            _accountService = accountService;
            _userService = userService;
            _propertyService = propertyService;
        }

        #region Add
        public async Task<RegisterResponse> Add(SaveUserViewModel vm, string origin)
        {
            RegisterResponse response = await _userService.RegisterAsync(vm, origin);

            if (!response.HasError)
            {

                var user = await _userService.GetByEmailAsync(vm.Email);
                var activeUser = await UpdateUserStatus(user.UserName);

                if (!activeUser.HasError)
                {
#pragma warning disable CS8601 // Possible null reference assignment.
                    SaveDeveloperViewModel saveDeveloperViewModel = new()
                    {
                        UserId = user.Id,
                        Identification = vm.Identification,
                        CreatedBy = "DefaultAppUser",
                        CreatedDate = DateTime.Now
                    };
#pragma warning restore CS8601 // Possible null reference assignment.

                    await base.Add(saveDeveloperViewModel);
                }
                else
                {
                    response.HasError = activeUser.HasError;
                    response.Error = activeUser.Error;
                }
            }

            return response;
        }
        #endregion

        #region Update
        public async Task<UpdateUserResponse> Update(SaveUserViewModel vm)
        {
            UpdateUserResponse response = await _userService.UpdateUserAsync(vm);

            if (!response.HasError)
            {
                var admiList = await base.GetAllViewModel();
                var developer = admiList.Find(developer => developer.UserId == vm.Id);


                SaveDeveloperViewModel saveDeveloperViewModel = await base.GetByIdSaveViewModel(developer.Id);

#pragma warning disable CS8601 // Possible null reference assignment.
                saveDeveloperViewModel.Identification = vm.Identification;
#pragma warning restore CS8601 // Possible null reference assignment.

                await base.Update(saveDeveloperViewModel, developer.Id);
            }

            return response;
        }
        #endregion

        #region GetAll
        public override async Task<List<DeveloperViewModel>> GetAllViewModel()
        {
            List<DeveloperViewModel> developerList = await base.GetAllViewModel();
            List<UserDTO> userDeveloperList = await _accountService.GetAllDeveloperAsync();

            foreach (var developer in developerList)
            {
                var user = userDeveloperList.Find(user => user.Id == developer.UserId);


                developer.FirstName = user.FirstName;

                developer.LastName = user.LastName;
                developer.Username = user.UserName;
                developer.Email = user.Email;
                developer.EmailConfirmed = user.EmailConfirmed;
            }

            return developerList;
        }
        #endregion

        #region GetByIdViewModel
        public override async Task<DeveloperViewModel> GetByIdViewModel(int id)
        {
            List<DeveloperViewModel> developerList = await GetAllViewModel();

#pragma warning disable CS8603 // Possible null reference return.
            return developerList.Find(developer => developer.Id == id);
#pragma warning restore CS8603 // Possible null reference return.
        }
        #endregion

        #region UpdateUserStatus
        public async Task<GenericResponse> UpdateUserStatus(string username)
        {
            var userStatus = await _accountService.UpdateUserStatusAsync(username);

            return userStatus;
        }
        #endregion
    }
}
