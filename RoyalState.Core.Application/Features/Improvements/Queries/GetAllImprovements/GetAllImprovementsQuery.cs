using AutoMapper;
using MediatR;
using RoyalState.Core.Application.DTOs.TypeDTO;
using RoyalState.Core.Application.Exceptions;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RoyalState.Core.Application.Features.Improvements.Queries.GetAllImprovements
{

    public class GetAllImprovementsQuery : IRequest<Response<IList<TypeDTO>>>
    {
    }

    public class GetAllImprovementsQueryHandler : IRequestHandler<GetAllImprovementsQuery, Response<IList<TypeDTO>>>
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;

        public GetAllImprovementsQueryHandler(IImprovementRepository improvementRepository, IMapper mapper)
        {
            _improvementRepository = improvementRepository;
            _mapper = mapper;
        }

        public async Task<Response<IList<TypeDTO>>> Handle(GetAllImprovementsQuery request, CancellationToken cancellationToken)
        {
            var improvements = await GetAllPropertyTypes();
            if (improvements == null) throw new ApiException($"Improvement not found", (int)HttpStatusCode.NotFound);
            return new Response<IList<TypeDTO>>(improvements);
        }

        private async Task<List<TypeDTO>> GetAllPropertyTypes()
        {
            var improvementsList = await _improvementRepository.GetAllAsync();

            if (improvementsList == null || improvementsList.Count == 0) throw new ApiException($"Improvement not found."
               , (int)HttpStatusCode.NotFound);

            var improvementDTOS = _mapper.Map<List<TypeDTO>>(improvementsList);



            return improvementDTOS;
        }
    }
}
