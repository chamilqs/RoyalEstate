using AutoMapper;
using MediatR;
using RoyalState.Core.Application.DTOs.Agent;
using RoyalState.Core.Application.DTOs.Property;
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

namespace RoyalState.Core.Application.Features.Properties.Queries.GetPropertyById
{

    /// <summary>
    /// Parameters to filter properties by id 
    /// </summary>  
    public class GetPropertyByIdQuery : IRequest<Response<PropertyDTO>>
    {
        [SwaggerParameter(Description = "Insert the id of the property to obtain. ")]
        [Required]
        public int Id { get; set; }
    }
    public class GetPropertyByIdQueryHandler : IRequestHandler<GetPropertyByIdQuery, Response<PropertyDTO>>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyService _propertyService;
        private readonly IMapper _mapper;

        public GetPropertyByIdQueryHandler(IPropertyRepository propertyRepository, IPropertyService propertyService, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _propertyService = propertyService;
            _mapper = mapper;
        }

        public async Task<Response<PropertyDTO>> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                return new Response<PropertyDTO>("Invalid property id.");
            }

            var property = await GetById(request.Id);

            if (property == null)
            {
                return new Response<PropertyDTO>("Property not found.");
            }

            return new Response<PropertyDTO>(property);
        }
        private async Task<PropertyDTO> GetById(int id)
        {
            var property = await _propertyService.GetByIdViewModel(id);

            if (property == null) return null;

            PropertyDTO propertyDTO = new PropertyDTO
            {
                Id = property.Id,
                SaleType = property.SaleTypeName,
                PropertyType = property.PropertyTypeName,
                AgentFirstName = property.AgentFirstName,
                AgentId = property.AgentId,
                Bathrooms = property.Bathrooms,
                Bedrooms = property.Bedrooms,
                Code = property.Code,
                Description = property.Description,
                Improvements = property.Improvements,
                Meters = property.Meters,
                Price = property.Price
            };

            return propertyDTO;

        }
    }
}
