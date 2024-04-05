using RoyalState.Core.Application.ViewModels.SaleTypes;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Interfaces.Services
{
    public interface ISaleTypeService : IGenericService<SaveSaleTypeViewModel, SaleTypeViewModel, SaleType>
    {
    }
}
