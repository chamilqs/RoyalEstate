using AutoMapper;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.SaleTypes;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Services
{
    public class SaleTypeService : GenericService<SaveSaleTypeViewModel, SaleTypeViewModel, SaleType>, ISaleTypeService
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;

        public SaleTypeService(ISaleTypeRepository saleTypeRepository, IMapper mapper) : base(saleTypeRepository, mapper)
        {
            _mapper = mapper;
            _saleTypeRepository = saleTypeRepository;
        }

        public override async Task<SaleTypeViewModel> GetByIdViewModel(int id)
        {
            var saleTypeList = await GetAllViewModelWithInclude();

#pragma warning disable CS8603 // Possible null reference return.
            return saleTypeList.FirstOrDefault(saleType => saleType.Id == id);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<List<SaleTypeViewModel>> GetAllViewModelWithInclude()
        {
            var saleTypeList = await _saleTypeRepository.GetAllWithIncludeAsync(new List<string> { "Properties" });


            return saleTypeList.Select(saleType => new SaleTypeViewModel
            {
                Id = saleType.Id,
                Name = saleType.Name,
                Description = saleType.Description,
                PropertiesQuantity = saleType.Properties.Count
            }).ToList();

        }
    }
}
