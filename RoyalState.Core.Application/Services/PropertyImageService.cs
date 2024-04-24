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
        private readonly IFileService _fileService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse user;
        private readonly IMapper _mapper;

        public PropertyImageService(IPropertyImageRepository propertyImageRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper, IUserService userService, IFileService fileService) : base(propertyImageRepository, mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _propertyImageRepository = propertyImageRepository;
            _mapper = mapper;

            user = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");

            _userService = userService;
            _fileService = fileService;
        }

        #region Get Methods

        #region GetImagesUrlByPropertyId
        /// <summary>
        /// Retrieves the URLs of images by property ID.
        /// </summary>
        /// <param name="propertyId">The ID of the property.</param>
        /// <returns>A list of image URLs.</returns>
        public async Task<List<string>> GetImagesUrlByPropertyId(int propertyId)
        {
            var propertyImageList = await GetAllViewModel();
            var thisProperty = propertyImageList.Where(p => p.PropertyId == propertyId);

            List<string> propertyImages = new();
            foreach (var image in thisProperty)
            {
                propertyImages.Add(image.ImageUrl);
            }

            return propertyImages;
        }
        #endregion

        #region GetPropertyImagesByPropertyId
        /// <summary>
        /// Retrieves the property images by property ID.
        /// </summary>
        /// <param name="propertyId">The ID of the property.</param>
        /// <returns>A list of property image view models.</returns>
        public async Task<List<PropertyImageViewModel>> GetPropertyImagesByPropertyId(int propertyId)
        {
            var propertyImagesList = await GetAllViewModel();
            var thisProperty = propertyImagesList.Where(p => p.PropertyId == propertyId).ToList();

            return thisProperty;

        }
        #endregion

        #endregion

        #region DeleteImagesByPropertyId
        /// <summary>
        /// Deletes the property images by property ID.
        /// </summary>
        /// <param name="propertyId">The ID of the property.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteImagesByPropertyId(int propertyId)
        {
            var propertyImages = await GetPropertyImagesByPropertyId(propertyId);
            var imagesToDelete = new List<string>();

            foreach (var image in propertyImages)
            {
                await Delete(image.Id);
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{image.ImageUrl}");
                if (File.Exists(imagePath))
                {
                    imagesToDelete.Add(imagePath);
                }
            }

            foreach (var imagePath in imagesToDelete)
            {
                await _fileService.DeleteFileAsync(imagePath);
            }
        }
        #endregion

        #region DeleteImagesUrlsByPropertyId
        /// <summary>
        /// Deletes the property images by property ID.
        /// </summary>
        /// <param name="propertyId">The ID of the property.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteImagesUrlsByPropertyId(int propertyId)
        {
            var propertyImages = await GetPropertyImagesByPropertyId(propertyId);
            var imagesToDelete = new List<string>();

            foreach (var image in propertyImages)
            {
                await Delete(image.Id);

            }

        }
        #endregion

    }
}
