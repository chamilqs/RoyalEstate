using AutoMapper;
using MediatR;
using RoyalState.Core.Application.DTOs.Property;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

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
            if (properties == null) return new Response<IList<PropertyDTO>>("Properties not found");

            return new Response<IList<PropertyDTO>>(properties);
        }

        private async Task<List<PropertyDTO>> GetAllPropertiesByAgentId(int agentId)
        {
            var propertyList = await _propertyService.GetAllViewModel();

            if (propertyList == null || propertyList.Count == 0) return null;


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
