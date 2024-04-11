using AutoMapper;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.PropertyImprovement;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Services
{
    public class PropertyImprovementService : GenericService<SavePropertyImprovementViewModel, PropertyImprovementViewModel, PropertyImprovement>, IPropertyImprovementService
    {
        private readonly IPropertyImprovementRepository _propertyImprovementRepository;
        private readonly IImprovementService _improvementService;
        private readonly IMapper _mapper;

        public PropertyImprovementService(IPropertyImprovementRepository propertyImprovementRepository, IMapper mapper, IImprovementService improvementService) : base(propertyImprovementRepository, mapper)
        {
            _mapper = mapper;
            _propertyImprovementRepository = propertyImprovementRepository;
            _improvementService = improvementService;
        }


        public async Task<List<string>> GetImprovementsByPropertyId(int propertyId)
        {
            var propertyImprovementsList = await GetAllViewModel();
            propertyImprovementsList.Where(p => p.PropertyId == propertyId).ToList();
            
            List<string> improvements = new();
            foreach (var propertyImprovement in propertyImprovementsList)
            {
                var improvementViewModel = await _improvementService.GetByIdViewModel(propertyImprovement.ImprovementId);
                improvements.Add(improvementViewModel.Name);
            }

            return improvements;
        }
    }
}
