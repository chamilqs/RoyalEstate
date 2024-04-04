using RoyalState.Core.Application.ViewModels.SalesTypes;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Interfaces.Services
{
    public interface ISalesTypeService : IGenericService<SaveSalesTypeViewModel, SalesTypeViewModel, SaleType>
    {
    }
}
