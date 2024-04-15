using MediatR;
using RoyalState.Core.Application.Exceptions;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RoyalState.Core.Application.Features.SaleTypes.Commands.DeleteSaleType
{

    /// <summary>
    /// Parameters for deleting a sale type
    /// </summary> 
    public class DeleteSaleTypeByIdCommand : IRequest<Response<int>>
    {
        [SwaggerParameter(Description = "Id of the sale type to delete")]
        public int Id { get; set; }
    }

    public class DeleteSaleTypeByIdCommandCommandHandler : IRequestHandler<DeleteSaleTypeByIdCommand, Response<int>>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;


        public DeleteSaleTypeByIdCommandCommandHandler(ISaleTypeRepository saleTypeRepository)
        {
            _saleTypeRepository = saleTypeRepository;
        }

        public async Task<Response<int>> Handle(DeleteSaleTypeByIdCommand command, CancellationToken cancellationToken)
        {
            var saleType = await _saleTypeRepository.GetByIdAsync(command.Id);
            if (saleType == null) throw new ApiException($"Sale Type not found.", (int)HttpStatusCode.NotFound);

            await _saleTypeRepository.DeleteAsync(saleType);

            return new Response<int>(saleType.Id);
        }
    }
}
