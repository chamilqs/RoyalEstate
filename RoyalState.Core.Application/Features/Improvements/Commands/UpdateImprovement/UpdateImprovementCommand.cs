using AutoMapper;
using MediatR;
using RoyalState.Core.Application.Exceptions;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Wrappers;
using RoyalState.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RoyalState.Core.Application.Features.Improvements.Commands.UpdateImprovement
{

    /// <summary>
    /// Parameters for updating an improvement
    /// </summary> 
    public class UpdateImprovementCommand : IRequest<Response<ImprovementUpdateResponse>>
    {
        [SwaggerParameter(Description = "Id of the improvement to update")]
        public int Id { get; set; }

        /// <example>Elevator</example>
        [SwaggerParameter(Description = "New name of the improvement")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Name { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <example>Moves people or goods between different levels of a building </example>
        [SwaggerParameter(Description = "New description of the improvement")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Description { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.



    }
    public class UpdateImprovementCommandHandler : IRequestHandler<UpdateImprovementCommand, Response<ImprovementUpdateResponse>>
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;

        public UpdateImprovementCommandHandler(IImprovementRepository improvementRepository, IMapper mapper)
        {
            _improvementRepository = improvementRepository;
            _mapper = mapper;
        }

        public async Task<Response<ImprovementUpdateResponse>> Handle(UpdateImprovementCommand command, CancellationToken cancellationToken)
        {
            var improvement = await _improvementRepository.GetByIdAsync(command.Id);

            if (improvement == null) throw new ApiException($"Improvement type not found.", (int)HttpStatusCode.NotFound);

            improvement = _mapper.Map<Improvement>(command);

            await _improvementRepository.UpdateAsync(improvement, improvement.Id);

            var improvementResponse = _mapper.Map<ImprovementUpdateResponse>(improvement);

            return new Response<ImprovementUpdateResponse>(improvementResponse);
        }
    }
}
