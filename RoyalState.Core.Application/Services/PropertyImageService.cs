using AutoMapper;
using Microsoft.AspNetCore.Http;
using RoyalState.Core.Application.DTOs.Account;
using RoyalState.Core.Application.Helpers;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.PropertyImage;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Services
{
    public class PropertyImageService : GenericService<SavePropertyImageViewModel, PropertyImageViewModel, PropertyImage>, IPropertyImageService
    {
        private readonly IPropertyImageRepository _propertyImageRepository;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse user;
        private readonly IMapper _mapper;

        public PropertyImageService(IPropertyImageRepository propertyImageRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper, IUserService userService) : base(propertyImageRepository, mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _propertyImageRepository = propertyImageRepository;
            _mapper = mapper;
            user = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _userService = userService;
        }

        #region GetImagesByPropertyId
        public async Task<List<string>> GetImagesByPropertyId(int propertyId)
        {
            var propertyImageList = await GetAllViewModel();
            propertyImageList.Where(p => p.PropertyId == propertyId);

            List<string> propertyImages = new();
            foreach (var image in propertyImageList)
            {
                propertyImages.Add(image.ImageUrl);
            }

            return propertyImages;
        }
        #endregion

    }
}
