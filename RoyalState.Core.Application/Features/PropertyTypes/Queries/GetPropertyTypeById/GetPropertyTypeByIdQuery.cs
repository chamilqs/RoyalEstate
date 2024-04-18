using AutoMapper;
using MediatR;
using RoyalState.Core.Application.DTOs.TypeDTO;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace RoyalState.Core.Application.Features.PropertyTypes.Queries.GetPropertyTypeById
{

    /// <summary>
    /// Parameters to filter property types by id 
    /// </summary>  
    public class GetPropertyTypeByIdQuery : IRequest<Response<TypeDTO>>
    {
        [SwaggerParameter(Description = "Insert the id of the property type to obtain. ")]
        [Required]
        public int Id { get; set; }
    }
    public class GetPropertyTypeByIdQueryHandler : IRequestHandler<GetPropertyTypeByIdQuery, Response<TypeDTO>>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IMapper _mapper;

        public GetPropertyTypeByIdQueryHandler(IPropertyTypeRepository propertyTypeRepository, IMapper mapper)
        {
            _propertyTypeRepository = propertyTypeRepository;
            _mapper = mapper;
        }

        public async Task<Response<TypeDTO>> Handle(GetPropertyTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var propertyType = await GetById(request.Id);
            if (propertyType == null) return new Response<TypeDTO>("Property type not found");
            return new Response<TypeDTO>(propertyType);
        }
        private async Task<TypeDTO> GetById(int id)
        {
            var propertyType = await _propertyTypeRepository.GetByIdAsync(id);

#pragma warning disable CS8603 // Possible null reference return.
            if (propertyType == null) return null;
#pragma warning restore CS8603 // Possible null reference return.

            var propertyTypeDTO = _mapper.Map<TypeDTO>(propertyType);

            return propertyTypeDTO;

        }
    }
}
