using AutoMapper;
using MediatR;
using RoyalState.Core.Application.Exceptions;
using RoyalState.Core.Application.Features.PropertyTypes.Commands.UpdatePropertyType;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.Wrappers;
using RoyalState.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
        public string Name { get; set; }


        /// <example>Temporary use or occupation of property</example>
        [SwaggerParameter(Description = "New description of the sale type")]
        public string Description { get; set; }



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
