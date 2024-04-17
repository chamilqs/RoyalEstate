using AutoMapper;
using MediatR;
using RoyalState.Core.Application.DTOs.Property;
using RoyalState.Core.Application.Exceptions;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RoyalState.Core.Application.Features.Properties.Queries.GetAllProperties
{

    public class GetAllPropertiesQuery : IRequest<Response<IList<PropertyDTO>>>
    {
    }

    public class GetAllPropertiesQueryHandler : IRequestHandler<GetAllPropertiesQuery, Response<IList<PropertyDTO>>>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;
        private readonly IPropertyService _propertyService;

        public GetAllPropertiesQueryHandler(IPropertyRepository propertyRepository, IMapper mapper, IPropertyService propertyService)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
            _propertyService = propertyService;
        }

        public async Task<Response<IList<PropertyDTO>>> Handle(GetAllPropertiesQuery request, CancellationToken cancellationToken)
        {
            var properties = await GetAllProperties();
            if (properties == null) throw new ApiException($"Agents not found", (int)HttpStatusCode.NoContent);
            return new Response<IList<PropertyDTO>>(properties);
        }

        private async Task<List<PropertyDTO>> GetAllProperties()
        {
            var propertyList = await _propertyService.GetAllViewModelApi();

            if (propertyList == null || propertyList.Count == 0) throw new ApiException($"Properties not found."
               , (int)HttpStatusCode.NoContent);

            var propertyDtos = new List<PropertyDTO>();

            foreach (var prop in propertyList)
            {

                var propertyDTO = new PropertyDTO
                {
                    Id = prop.Id,
                    SaleType = prop.SaleTypeName,
                    AgentFirstName = prop.AgentFirstName,
                    AgentId = prop.AgentId,
                    Bathrooms = prop.Bathrooms,
                    Bedrooms = prop.Bedrooms,
                    Code = prop.Code,
                    Description = prop.Description,
                    Improvements = prop.Improvements,
                    Meters = prop.Meters,
                    Price = prop.Price,
                    PropertyType = prop.PropertyTypeName
                };

                propertyDtos.Add(propertyDTO);

            }

            return propertyDtos;
        }
    }
}

