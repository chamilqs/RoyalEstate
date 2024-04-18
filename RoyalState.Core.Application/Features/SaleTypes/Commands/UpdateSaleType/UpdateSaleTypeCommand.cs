using AutoMapper;
using MediatR;
using RoyalState.Core.Application.Exceptions;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.Wrappers;
using RoyalState.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RoyalState.Core.Application.Features.SaleTypes.Commands.UpdateSaleType
{

    /// <summary>
    /// Parameters for updating a sale type
    /// </summary> 
    public class UpdateSaleTypeCommand : IRequest<Response<SaleTypeUpdateResponse>>
    {
        [SwaggerParameter(Description = "Id of the sale type to update")]
        public int Id { get; set; }

        /// <example>Rent</example>
        [SwaggerParameter(Description = "New name of the sale type")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Name { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


        /// <example>Temporary use or occupation of property</example>
        [SwaggerParameter(Description = "New description of the sale type")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Description { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.



    }
    public class UpdateSaleTypeCommandHandler : IRequestHandler<UpdateSaleTypeCommand, Response<SaleTypeUpdateResponse>>
    {
        private readonly ISaleTypeService _saleTypeService;
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;

        public UpdateSaleTypeCommandHandler(ISaleTypeService saleTypeService, ISaleTypeRepository saleTypeRepository, IMapper mapper)
        {
            _saleTypeService = saleTypeService;
            _saleTypeRepository = saleTypeRepository;
            _mapper = mapper;
        }

        public async Task<Response<SaleTypeUpdateResponse>> Handle(UpdateSaleTypeCommand command, CancellationToken cancellationToken)
        {
            var saleType = await _saleTypeRepository.GetByIdAsync(command.Id);

            if (saleType == null) throw new ApiException($"Sale Type not found.", (int)HttpStatusCode.NotFound);

            saleType = _mapper.Map<SaleType>(command);

            await _saleTypeRepository.UpdateAsync(saleType, saleType.Id);

            var saleTypeResponse = _mapper.Map<SaleTypeUpdateResponse>(saleType);

            return new Response<SaleTypeUpdateResponse>(saleTypeResponse);
        }
    }
}
