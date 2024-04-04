using AutoMapper;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.SalesTypes;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Services
{
    public class SalesTypeService : GenericService<SaveSalesTypeViewModel, SalesTypeViewModel, SaleType>, ISalesTypeService
    {
        private readonly ISalesTypeRepository _salesTypeRepository;
        private readonly IMapper _mapper;

        public SalesTypeService(ISalesTypeRepository salesTypeRepository, IMapper mapper) : base(salesTypeRepository, mapper)
        {
            _mapper = mapper;
            _salesTypeRepository = salesTypeRepository;
        }
    }
}
