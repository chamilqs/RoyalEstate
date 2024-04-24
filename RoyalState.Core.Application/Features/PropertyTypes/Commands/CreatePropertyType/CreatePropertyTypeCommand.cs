using AutoMapper;
using MediatR;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.Wrappers;
using RoyalState.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace RoyalState.Core.Application.Features.PropertyTypes.Commands.CreatePropertyType
{
    /// <summary>
    /// Parameters for creating a property type
    /// </summary> 
    public class CreatePropertyTypeCommand : IRequest<Response<int>>
    {
        /// <example>Apartment</example>
        [SwaggerParameter(Description = "Name of the property type")]

        public string Name { get; set; }


        /// <example>An apartment in a small building or tower </example>
        [SwaggerParameter(Description = "Description of the property type")]

        public string Description { get; set; }

    }
    public class CreatePropertyTypeCommandHandler : IRequestHandler<CreatePropertyTypeCommand, Response<int>>
    {
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly IPropertyTypeRepository _repository;
        private readonly IMapper _mapper;

        public CreatePropertyTypeCommandHandler(IPropertyTypeService propertyTypeService, IPropertyTypeRepository repository, IMapper mapper)
        {
            _propertyTypeService = propertyTypeService;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreatePropertyTypeCommand command, CancellationToken cancellationToken)
        {
            var propertyType = _mapper.Map<PropertyType>(command);
            propertyType = await _repository.AddAsync(propertyType);
            return new Response<int>(propertyType.Id);
        }
    }
}
