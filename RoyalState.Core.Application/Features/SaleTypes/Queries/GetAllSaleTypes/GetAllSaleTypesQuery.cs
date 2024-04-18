using AutoMapper;
using MediatR;
using RoyalState.Core.Application.DTOs.TypeDTO;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Wrappers;

namespace RoyalState.Core.Application.Features.SaleTypes.Queries.GetAllSaleTypes
{

    public class GetAllSaleTypesQuery : IRequest<Response<IList<TypeDTO>>>
    {
    }

    public class GetAllSaleTypesQueryHandler : IRequestHandler<GetAllSaleTypesQuery, Response<IList<TypeDTO>>>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;

        public GetAllSaleTypesQueryHandler(ISaleTypeRepository saleTypeRepository, IMapper mapper)
        {
            _saleTypeRepository = saleTypeRepository;
            _mapper = mapper;
        }

        public async Task<Response<IList<TypeDTO>>> Handle(GetAllSaleTypesQuery request, CancellationToken cancellationToken)
        {
            var saleTypes = await GetAllSaleTypes();
            if (saleTypes == null) return new Response<IList<TypeDTO>>("Sale type not found");
            return new Response<IList<TypeDTO>>(saleTypes);
        }

        private async Task<List<TypeDTO>> GetAllSaleTypes()
        {
            var saleTypesList = await _saleTypeRepository.GetAllAsync();

#pragma warning disable CS8603 // Possible null reference return.
            if (saleTypesList == null || saleTypesList.Count == 0) return null;
#pragma warning restore CS8603 // Possible null reference return.

            var propertyDtos = _mapper.Map<List<TypeDTO>>(saleTypesList);



            return propertyDtos;
        }
    }
}
