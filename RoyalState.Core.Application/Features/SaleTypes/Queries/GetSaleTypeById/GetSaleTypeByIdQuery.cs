using AutoMapper;
using MediatR;
using RoyalState.Core.Application.DTOs.TypeDTO;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace RoyalState.Core.Application.Features.SaleTypes.Queries.GetSaleTypeById
{

    /// <summary>
    /// Parameters to filter sale types by id 
    /// </summary>  
    public class GetSaleTypeByIdQuery : IRequest<Response<TypeDTO>>
    {
        [SwaggerParameter(Description = "Insert the id of the sale type to obtain. ")]
        [Required]
        public int Id { get; set; }
    }
    public class GetSaleTypeByIdQueryHandler : IRequestHandler<GetSaleTypeByIdQuery, Response<TypeDTO>>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;

        public GetSaleTypeByIdQueryHandler(ISaleTypeRepository saleTypeRepository, IMapper mapper)
        {
            _saleTypeRepository = saleTypeRepository;
            _mapper = mapper;
        }

        public async Task<Response<TypeDTO>> Handle(GetSaleTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var saleType = await GetById(request.Id);
            if (saleType == null) return new Response<TypeDTO>("Sale type not found");
            return new Response<TypeDTO>(saleType);
        }
        private async Task<TypeDTO> GetById(int id)
        {
            var saleType = await _saleTypeRepository.GetByIdAsync(id);


            if (saleType == null) return null;


            var propertyDTO = _mapper.Map<TypeDTO>(saleType);

            return propertyDTO;

        }
    }
}
