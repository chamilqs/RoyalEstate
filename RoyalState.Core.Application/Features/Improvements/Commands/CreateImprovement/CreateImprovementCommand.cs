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
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Name { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <example>Moves people or goods between different levels of a building </example>
        [SwaggerParameter(Description = "Description of the improvement")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Description { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
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