using AutoMapper;
using MediatR;
using RoyalState.Core.Application.DTOs.Agent;
using RoyalState.Core.Application.DTOs.Property;
using RoyalState.Core.Application.Exceptions;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.Wrappers;
using RoyalState.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RoyalState.Core.Application.Features.Agents.Queries.GetAgentPropertyById
{

    /// <summary>
    /// Parameters to filter properties by id of the agent
    /// </summary>  
    public class GetAgentPropertyByIdQuery : IRequest<Response<IList<PropertyDTO>>>
    {
        [SwaggerParameter(Description = "Insert the id of the agent to obtain its properties. ")]
        [Required]
        public int AgentId { get; set; }
    }
    public class GetAgentPropertyByIdQueryHandler : IRequestHandler<GetAgentPropertyByIdQuery, Response<IList<PropertyDTO>>>
    {
        private readonly IAgentRepository _agentRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyService _propertyService;
        private readonly IMapper _mappper;
        private readonly IAccountService _accountService;

        public GetAgentPropertyByIdQueryHandler(IAgentRepository agentRepository, IPropertyService propertyService, IMapper mappper)
        {
            _agentRepository = agentRepository;
            _mappper = mappper;
            _propertyService = propertyService;
        }

        public async Task<Response<IList<PropertyDTO>>> Handle(GetAgentPropertyByIdQuery request, CancellationToken cancellationToken)
        {
            var properties = await GetAllPropertiesByAgentId(request.AgentId);
            if (properties == null) throw new ApiException($"Properties not found.", (int)HttpStatusCode.NotFound);

            return new Response<IList<PropertyDTO>>(properties);
        }

        private async Task<List<PropertyDTO>> GetAllPropertiesByAgentId(int agentId)
        {
            var propertyList = await _propertyService.GetAllViewModel();
            if (propertyList == null || propertyList.Count == 0) throw new ApiException($"Properties not found."
                , (int)HttpStatusCode.NotFound);

            var propertiesDTOs = new List<PropertyDTO>();

            foreach (var property in propertyList.Where(p => p.AgentId == agentId))
            {
                var propertyDTO = new PropertyDTO
                {
                    Id = property.Id,
                    AgentId = property.AgentId,
                    SaleType = property.SaleTypeName,
                    Bathrooms = property.Bathrooms,
                    Bedrooms = property.Bedrooms,
                    Code = property.Code,
                    Description = property.Description,
                    Meters = property.Meters,
                    Price = property.Price,
                    PropertyType = property.PropertyTypeName,
                    AgentFirstName = property.AgentFirstName,
                    Improvements = property.Improvements,
                };
                propertiesDTOs.Add(propertyDTO);
            }
            {

                return propertiesDTOs;
            }


        }
    }
}
