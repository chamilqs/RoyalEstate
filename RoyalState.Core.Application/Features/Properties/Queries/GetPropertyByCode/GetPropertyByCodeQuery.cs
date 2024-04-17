using AutoMapper;
using MediatR;
using RoyalState.Core.Application.DTOs.Property;
using RoyalState.Core.Application.Exceptions;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.Property;
using RoyalState.Core.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RoyalState.Core.Application.Features.Properties.Queries.GetPropertyByCode
{
    public class GetPropertyByCodeQuery : IRequest<Response<PropertyDTO>>
    {
        public string? Code { get; set; }
    }

    public class GetPropertyByCodeQueryHandler : IRequestHandler<GetPropertyByCodeQuery, Response<PropertyDTO>>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyService _propertyService;
        private readonly IMapper _mapper;

        public GetPropertyByCodeQueryHandler(IPropertyRepository propertyRepository, IPropertyService propertyService, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _propertyService = propertyService;
            _mapper = mapper;
        }

        public async Task<Response<PropertyDTO>> Handle(GetPropertyByCodeQuery request, CancellationToken cancellationToken)
        {
            var filter = _mapper.Map<GetPropertyByCodeParameter>(request);
            var property = await GetByCode(filter);
            if (property == null) throw new ApiException($"Property not found.", (int)HttpStatusCode.NoContent);
            return new Response<PropertyDTO>(property);
        }

        private async Task<PropertyDTO> GetByCode(GetPropertyByCodeParameter filter)
        {
            var property = await _propertyService.GetPropertyByCode(filter.Code);

            if (property == null) throw new ApiException($"Property not found."
               , (int)HttpStatusCode.NoContent);

            PropertyDTO propertyDTO = new PropertyDTO
            {
                Code = property.Code,
                Id = property.Id,
                SaleType = property.SaleTypeName,
                AgentFirstName = property.AgentFirstName,
                AgentId = property.AgentId,
                Bathrooms = property.Bathrooms,
                Bedrooms = property.Bedrooms,
                Description = property.Description,
                Improvements = property.Improvements,
                Meters = property.Meters,
                Price = property.Price,
                PropertyType = property.PropertyTypeName

            };

            return propertyDTO;

        }
    }
}
