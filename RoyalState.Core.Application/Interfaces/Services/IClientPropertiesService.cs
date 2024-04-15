using RoyalState.Core.Application.ViewModels.ClientProperties;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Interfaces.Services
{
    public interface IClientPropertiesService : IGenericService<SaveClientPropertiesViewModel, ClientPropertiesViewModel, ClientProperties>
    {
        Task<ClientPropertiesViewModel> GetByPropertyIdViewModel(int propertyId);
    }
}
