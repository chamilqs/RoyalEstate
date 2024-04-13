using RoyalState.Core.Application.ViewModels.PropertyImage;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Interfaces.Services
{
    public interface IPropertyImageService : IGenericService<SavePropertyImageViewModel, PropertyImageViewModel, PropertyImage>
    {
        Task<List<string>> GetImagesUrlByPropertyId(int propertyId);
        Task DeleteImagesByPropertyId(int propertyId);
        Task<List<PropertyImageViewModel>> GetPropertyImagesByPropertyId(int propertyId);

    }
}
