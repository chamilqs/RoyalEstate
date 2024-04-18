using AutoMapper;
using MediatR;
using RoyalState.Core.Application.DTOs.Property;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.Wrappers;

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
            if (request.Code == null)
            {
                return new Response<PropertyDTO>("Invalid property code.");
            }

            var filter = _mapper.Map<GetPropertyByCodeParameter>(request);
            var property = await GetByCode(filter);
            if (property == null) return new Response<PropertyDTO>("Property not found.");
            return new Response<PropertyDTO>(property);
        }

        private async Task<PropertyDTO> GetByCode(GetPropertyByCodeParameter filter)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            var property = await _propertyService.GetPropertyByCode(filter.Code);
#pragma warning restore CS8604 // Possible null reference argument.

#pragma warning disable CS8603 // Possible null reference return.
            if (property == null) return null;
#pragma warning restore CS8603 // Possible null reference return.

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
