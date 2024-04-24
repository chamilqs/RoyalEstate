using MediatR;
using RoyalState.Core.Application.Exceptions;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RoyalState.Core.Application.Features.PropertyTypes.Commands.DeletePropertyTypeById
{

    /// <summary>
    /// Parameters for deleting a property type
    /// </summary> 
    public class DeletePropertyTypeByIdCommand : IRequest<Response<int>>
    {
        [SwaggerParameter(Description = "Id of the property type to delete")]
        public int Id { get; set; }
    }

    public class DeletePropertyTypeByIdCommandHandler : IRequestHandler<DeletePropertyTypeByIdCommand, Response<int>>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;

        public DeletePropertyTypeByIdCommandHandler(IPropertyTypeRepository propertyTypeRepository)
        {
            _propertyTypeRepository = propertyTypeRepository;
        }
        public async Task<Response<int>> Handle(DeletePropertyTypeByIdCommand command, CancellationToken cancellationToken)
        {
            var propertyType = await _propertyTypeRepository.GetByIdAsync(command.Id);
            if (propertyType == null) throw new ApiException($"Property Type not found.", (int)HttpStatusCode.NotFound);

            await _propertyTypeRepository.DeleteAsync(propertyType);

            return new Response<int>(propertyType.Id);
        }
    }
}
