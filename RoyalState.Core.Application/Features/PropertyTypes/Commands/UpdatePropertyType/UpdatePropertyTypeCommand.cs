using AutoMapper;
using MediatR;
using RoyalState.Core.Application.Exceptions;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Wrappers;
using RoyalState.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RoyalState.Core.Application.Features.PropertyTypes.Commands.UpdatePropertyType
{

    /// <summary>
    /// Parameters for updating a property type
    /// </summary> 
    public class UpdatePropertyTypeCommand : IRequest<Response<PropertyTypeUpdateResponse>>
    {
        [SwaggerParameter(Description = "Id of the property type to update")]
        public int Id { get; set; }

        /// <example>Apartment</example>
        [SwaggerParameter(Description = "New name of the property type")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Name { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


        /// <example>An apartment in a small building or tower </example>
        [SwaggerParameter(Description = "New description of the property type")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Description { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.



    }
    public class UpdatePropertyTypeCommandHandler : IRequestHandler<UpdatePropertyTypeCommand, Response<PropertyTypeUpdateResponse>>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IMapper _mapper;

        public UpdatePropertyTypeCommandHandler(IPropertyTypeRepository propertyTypeRepository, IMapper mapper)
        {
            _propertyTypeRepository = propertyTypeRepository;
            _mapper = mapper;
        }

        public async Task<Response<PropertyTypeUpdateResponse>> Handle(UpdatePropertyTypeCommand command, CancellationToken cancellationToken)
        {
            var propertyType = await _propertyTypeRepository.GetByIdAsync(command.Id);

            if (propertyType == null) throw new ApiException($"Property type not found.", (int)HttpStatusCode.NotFound);

            propertyType = _mapper.Map<PropertyType>(command);

            await _propertyTypeRepository.UpdateAsync(propertyType, propertyType.Id);

            var propertyTypeResponse = _mapper.Map<PropertyTypeUpdateResponse>(propertyType);

            return new Response<PropertyTypeUpdateResponse>(propertyTypeResponse);
        }
    }
}
