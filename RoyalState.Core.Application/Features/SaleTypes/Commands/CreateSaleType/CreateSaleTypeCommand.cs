using AutoMapper;
using MediatR;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.Wrappers;
using RoyalState.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalState.Core.Application.Features.SaleTypes.Commands.CreateSaleType
{
    /// <summary>
    /// Parameters for creating a sale type
    /// </summary> 
    public class CreateSaleTypeCommand : IRequest<Response<int>>
    {
        /// <example>Rent</example>
        [SwaggerParameter(Description = "Name of the sale type")]
        public string Name { get; set; }

        /// <example>Temporary use or occupation of property</example>
        [SwaggerParameter(Description = "Description of the sale type")]
        public string Description { get; set; }
    }
    public class CreateSaleTypeCommandHandler : IRequestHandler<CreateSaleTypeCommand, Response<int>>
    {
        private readonly ISaleTypeService _saleTypeService;
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;

        public CreateSaleTypeCommandHandler(ISaleTypeService saleTypeService, ISaleTypeRepository saleTypeRepository, IMapper mapper)
        {
            _saleTypeService = saleTypeService;
            _saleTypeRepository = saleTypeRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateSaleTypeCommand command, CancellationToken cancellationToken)
        {
            var saleType = _mapper.Map<SaleType>(command);
            saleType = await _saleTypeRepository.AddAsync(saleType);
            return new Response<int>(saleType.Id);
        }
    }
}
