using AutoMapper;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.ViewModels.Improvements;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Application.Services
{
    public class ImprovementService : GenericService<SaveImprovementViewModel, ImprovementViewModel, Improvement>, IImprovementService
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;

        public ImprovementService(IImprovementRepository improvementRepository, IMapper mapper) : base(improvementRepository, mapper)
        {
            _mapper = mapper;
            _improvementRepository = improvementRepository;
        }

        #region GetByNameViewModel
        /// <summary>
        /// Retrieves an ImprovementViewModel by name.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>The ImprovementViewModel with the matching name, or null if not found.</returns>
        public async Task<ImprovementViewModel> GetByNameViewModel(string name)
        {
            var improvements = await GetAllViewModel();
            return improvements.Where(n => n.Name.Contains(name)).FirstOrDefault();

        }
        #endregion
    }
}
