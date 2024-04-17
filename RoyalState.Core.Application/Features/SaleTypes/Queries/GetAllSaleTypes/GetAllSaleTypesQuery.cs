using AutoMapper;
using MediatR;
using RoyalState.Core.Application.DTOs.Property;
using RoyalState.Core.Application.DTOs.TypeDTO;
using RoyalState.Core.Application.Exceptions;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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

            if (saleTypesList == null || saleTypesList.Count == 0) return null;

            var propertyDtos = _mapper.Map<List<TypeDTO>>(saleTypesList);



            return propertyDtos;
        }
    }
}
