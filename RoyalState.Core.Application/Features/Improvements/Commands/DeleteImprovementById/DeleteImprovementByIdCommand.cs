using MediatR;
using RoyalState.Core.Application.Exceptions;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RoyalState.Core.Application.Features.Improvements.Commands.DeleteImprovementById
{

    /// <summary>
    /// Parameters for deleting an improvement
    /// </summary> 
    public class DeleteImprovementByIdCommand : IRequest<Response<int>>
    {
        [SwaggerParameter(Description = "Id of the improvement to delete")]
        public int Id { get; set; }
    }

    public class DeleteImprovementByIdCommandHandler : IRequestHandler<DeleteImprovementByIdCommand, Response<int>>
    {
        private readonly IImprovementRepository _improvementRepository;

        public DeleteImprovementByIdCommandHandler(IImprovementRepository improvementRepository)
        {
            _improvementRepository = improvementRepository;
        }

        public async Task<Response<int>> Handle(DeleteImprovementByIdCommand command, CancellationToken cancellationToken)
        {
            var improvement = await _improvementRepository.GetByIdAsync(command.Id);
            if (improvement == null) throw new ApiException($"Improvement  not found.", (int)HttpStatusCode.NotFound);

            await _improvementRepository.DeleteAsync(improvement);

            return new Response<int>(improvement.Id);
        }
    }
}
