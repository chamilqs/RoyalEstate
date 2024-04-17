using AutoMapper;
using MediatR;
using RoyalState.Core.Application.DTOs.TypeDTO;
using RoyalState.Core.Application.Exceptions;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RoyalState.Core.Application.Features.PropertyTypes.Queries.GetAllPropertyTypes
{

    public class GetAllPropertyTypeQuery : IRequest<Response<IList<TypeDTO>>>
    {
    }

    public class GetAllPropertyTypeQueryHandler : IRequestHandler<GetAllPropertyTypeQuery, Response<IList<TypeDTO>>>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IMapper _mapper;

        public GetAllPropertyTypeQueryHandler(IPropertyTypeRepository propertyTypeRepository, IMapper mapper)
        {
            _propertyTypeRepository = propertyTypeRepository;
            _mapper = mapper;
        }

        public async Task<Response<IList<TypeDTO>>> Handle(GetAllPropertyTypeQuery request, CancellationToken cancellationToken)
        {
            var propertyTypes = await GetAllPropertyTypes();
            if (propertyTypes == null) throw new ApiException($"Property Types not found", (int)HttpStatusCode.NoContent);
            return new Response<IList<TypeDTO>>(propertyTypes);
        }

        private async Task<List<TypeDTO>> GetAllPropertyTypes()
        {
            var propertyTypesList = await _propertyTypeRepository.GetAllAsync();

            if (propertyTypesList == null || propertyTypesList.Count == 0) throw new ApiException($"Property types not found."
               , (int)HttpStatusCode.NoContent);

            var propertyTypesDtos = _mapper.Map<List<TypeDTO>>(propertyTypesList);



            return propertyTypesDtos;
        }
    }
}
