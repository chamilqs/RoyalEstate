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
        private readonly IMapper _mappper;
        private readonly IAccountService _accountService;

        public GetAgentPropertyByIdQueryHandler(IAgentRepository agentRepository, IAccountService accountService, IPropertyRepository propertyRepository, IMapper mappper)
        {
            _agentRepository = agentRepository;
            _propertyRepository = propertyRepository;
            _mappper = mappper;
            _accountService = accountService;
        }

        public async Task<Response<IList<PropertyDTO>>> Handle(GetAgentPropertyByIdQuery request, CancellationToken cancellationToken)
        {
            var properties = await GetAllPropertiesByAgentId(request.AgentId);
            if (properties == null) throw new ApiException($"Properties not found.", (int)HttpStatusCode.NotFound);

            return new Response<IList<PropertyDTO>>(properties);
        }

        private async Task<List<PropertyDTO>> GetAllPropertiesByAgentId(int agentId)
        {
            var propertyList = await _propertyRepository.GetAllAsync();

            if (propertyList == null || propertyList.Count == 0) throw new ApiException($"Properties not found."
                , (int)HttpStatusCode.NotFound);

            var propertiesDTOs = new List<PropertyDTO>();

            foreach (var property in propertyList.Where(p => p.AgentId == agentId))
            {
                var agent = await _agentRepository.GetByIdAsync(property.AgentId);
                var agentUser = await _accountService.FindByIdAsync(agent.UserId);
                var propertyDTO = new PropertyDTO
                {
                    Id = property.Id,
                    AgentId = agentId,
                    SaleType = property.SaleType.Name,
                    Bathrooms = property.Bathrooms,
                    Bedrooms = property.Bedrooms,
                    Code = property.Code,
                    Description = property.Description,
                    Meters = property.Meters,
                    Price = property.Price,
                    PropertyType = property.PropertyType.Name,
                    AgentFirstName = agentUser.FirstName,
                    Improvements = property.Improvements.Select(i => i.Name).ToList(),
                };
                propertiesDTOs.Add(propertyDTO);
            }
            {

                return propertiesDTOs;
            }


        }
    }
