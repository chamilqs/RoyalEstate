using RoyalState.Core.Application.ViewModels.PropertyTypes;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Interfaces.Services
{
    public interface IPropertyTypeService : IGenericService<SavePropertyTypeViewModel, PropertyTypeViewModel, PropertyType>
    {
    }
}
