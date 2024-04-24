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

        #region Get Methods

        #region GetImprovementsNamesByPropertyId
        /// <summary>
        /// Retrieves the names of improvements associated with a property based on the property ID.
        /// </summary>
        /// <param name="propertyId">The ID of the property.</param>
        /// <returns>A list of improvement names.</returns>
        public async Task<List<string>> GetImprovementsNamesByPropertyId(int propertyId)
        {
            var propertyImprovementsList = await GetAllViewModel();
            var thisProperty = propertyImprovementsList.Where(p => p.PropertyId == propertyId).ToList();

            List<string> improvements = new();
            foreach (var propertyImprovement in thisProperty)
            {
                var improvementViewModel = await _improvementService.GetByIdViewModel(propertyImprovement.ImprovementId);
                improvements.Add(improvementViewModel.Name);
            }

            return improvements;
        }
        #endregion

        #region GetImprovementsByPropertyId
        /// <summary>
        /// Retrieves the improvements associated with a property based on the property ID.
        /// </summary>
        /// <param name="propertyId">The ID of the property.</param>
        /// <returns>A list of property improvement view models.</returns>
        public async Task<List<PropertyImprovementViewModel>> GetImprovementsByPropertyId(int propertyId)
        {
            var propertyImprovementsList = await GetAllViewModel();
            return propertyImprovementsList.Where(p => p.PropertyId == propertyId).ToList();
        }
        #endregion

        #endregion

        #region DeleteImprovementsByPropertyId
        /// <summary>
        /// Deletes the improvements associated with a property based on the property ID.
        /// </summary>
        /// <param name="propertyId">The ID of the property.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteImprovementsByPropertyId(int propertyId)
        {
            var propertyImprovements = await GetImprovementsByPropertyId(propertyId);
            foreach (var improvement in propertyImprovements)
            {
                await Delete(improvement.Id);
            }

        }
        #endregion

    }
}
