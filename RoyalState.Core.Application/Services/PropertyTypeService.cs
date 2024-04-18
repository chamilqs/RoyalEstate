using AutoMapper;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.PropertyTypes;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Services
{
    public class PropertyTypeService : GenericService<SavePropertyTypeViewModel, PropertyTypeViewModel, PropertyType>, IPropertyTypeService
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IMapper _mapper;

        public PropertyTypeService(IPropertyTypeRepository propertyTypeRepository, IMapper mapper) : base(propertyTypeRepository, mapper)
        {
            _mapper = mapper;
            _propertyTypeRepository = propertyTypeRepository;
        }

        public override async Task<PropertyTypeViewModel> GetByIdViewModel(int id)
        {
            var propertyTypeList = await GetAllViewModelWithInclude();

#pragma warning disable CS8603 // Possible null reference return.
            return propertyTypeList.FirstOrDefault(propertyType => propertyType.Id == id);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<List<PropertyTypeViewModel>> GetAllViewModelWithInclude()
        {
            var propertyTypeList = await _propertyTypeRepository.GetAllWithIncludeAsync(new List<string> { "Properties" });

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return propertyTypeList.Select(propertyType => new PropertyTypeViewModel
            {
                Id = propertyType.Id,
                Name = propertyType.Name,
                Description = propertyType.Description,
                PropertiesQuantity = propertyType.Properties.Count
            }).ToList();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}
