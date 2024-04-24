using AutoMapper;
using MediatR;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Wrappers;
using RoyalState.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace RoyalState.Core.Application.Features.Improvements.Commands.CreateImprovement
{

    /// <summary>
    /// Parameters for creating an improvement
    /// </summary> 
    public class CreateImprovementCommand : IRequest<Response<int>>
    {
        /// <example>Elevator</example>
        [SwaggerParameter(Description = "Name of the improvement")]

        public string Name { get; set; }


        /// <example>Moves people or goods between different levels of a building </example>
        [SwaggerParameter(Description = "Description of the improvement")]

        public string Description { get; set; }

    }
    public class CreateImprovementCommandHandler : IRequestHandler<CreateImprovementCommand, Response<int>>
    {

        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;

        public CreateImprovementCommandHandler(IImprovementRepository improvementRepository, IMapper mapper)
        {
            _improvementRepository = improvementRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateImprovementCommand command, CancellationToken cancellationToken)
        {
            var improvement = _mapper.Map<Improvement>(command);
            improvement = await _improvementRepository.AddAsync(improvement);
            return new Response<int>(improvement.Id);
        }
    }
}