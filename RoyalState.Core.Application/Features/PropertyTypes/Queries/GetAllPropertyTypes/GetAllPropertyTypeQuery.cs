using AutoMapper;
using MediatR;
using RoyalState.Core.Application.DTOs.TypeDTO;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Wrappers;

namespace RoyalState.Core.Application.Features.PropertyTypes.Queries.GetAllPropertyTypes
{

    public class GetAllPropertyTypeQuery : IRequest<Response<IList<TypeDTO>>>
    {
    }

    public class GetAllPropertyTypeQueryHandler : IRequestHandler<GetAllPropertyTypeQuery, Response<IList<TypeDTO>>>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IMapper _mapper;

        public GetAllPropertyTypeQueryHandler(IPropertyTypeRepository propertyTypeRepository, IMapper mapper)
        {
            _propertyTypeRepository = propertyTypeRepository;
            _mapper = mapper;
        }

        public async Task<Response<IList<TypeDTO>>> Handle(GetAllPropertyTypeQuery request, CancellationToken cancellationToken)
        {
            var propertyTypes = await GetAllPropertyTypes();
            if (propertyTypes == null) return new Response<IList<TypeDTO>>("Property types not found");
            return new Response<IList<TypeDTO>>(propertyTypes);
        }

        private async Task<List<TypeDTO>> GetAllPropertyTypes()
        {
            var propertyTypesList = await _propertyTypeRepository.GetAllAsync();


            if (propertyTypesList == null || propertyTypesList.Count == 0) return null;


            var propertyTypesDtos = _mapper.Map<List<TypeDTO>>(propertyTypesList);



            return propertyTypesDtos;
        }
    }
}
