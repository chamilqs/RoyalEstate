using AutoMapper;
using MediatR;
using RoyalState.Core.Application.DTOs.TypeDTO;
using RoyalState.Core.Application.Exceptions;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RoyalState.Core.Application.Features.Improvements.Queries.GetImprovementById
{

    /// <summary>
    /// Parameters to filter improvement by id 
    /// </summary>  
    public class GetImprovementByIdQuery : IRequest<Response<TypeDTO>>
    {
        [SwaggerParameter(Description = "Insert the id of the improvement to obtain.")]
        [Required]
        public int Id { get; set; }
    }
    public class GetImprovementByIdQueryHandler : IRequestHandler<GetImprovementByIdQuery, Response<TypeDTO>>
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;

        public GetImprovementByIdQueryHandler(IImprovementRepository improvementRepository, IMapper mapper)
        {
            _improvementRepository = improvementRepository;
            _mapper = mapper;
        }

        public async Task<Response<TypeDTO>> Handle(GetImprovementByIdQuery request, CancellationToken cancellationToken)
        {
            var improvement = await GetById(request.Id);
            if (improvement == null) throw new ApiException($"Improvement not found.", (int)HttpStatusCode.NotFound);
            return new Response<TypeDTO>(improvement);
        }
        private async Task<TypeDTO> GetById(int id)
        {
            var improvement = await _improvementRepository.GetByIdAsync(id);

            if (improvement == null) throw new ApiException($"Improvement not found."
               , (int)HttpStatusCode.NotFound);

            var improvementDTO = _mapper.Map<TypeDTO>(improvement);

            return improvementDTO;

        }
    }
}
