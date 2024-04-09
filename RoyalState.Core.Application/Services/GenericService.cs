using AutoMapper;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Application.Interfaces.Services;

namespace RoyalState.Core.Application.Services
{
    public class GenericService<SaveViewModel, ViewModel, Entity> : IGenericService<SaveViewModel, ViewModel, Entity>
        where SaveViewModel : class
        where ViewModel : class
        where Entity : class
    {
        private readonly IGenericRepository<Entity> _repository;
        private readonly IMapper _mapper;

        public GenericService(IGenericRepository<Entity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<SaveViewModel> Add(SaveViewModel viewModel)
        {
            Entity entity = _mapper.Map<Entity>(viewModel);

            entity = await _repository.AddAsync(entity);

            SaveViewModel saveVm = _mapper.Map<SaveViewModel>(entity);

            return saveVm;
        }

        public virtual async Task Update(SaveViewModel viewModel, int id)
        {
            Entity entity = _mapper.Map<Entity>(viewModel);

            await _repository.UpdateAsync(entity, id);
        }

        public virtual async Task Delete(int id)
        {
            Entity entity = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(entity);
        }

        public virtual async Task<SaveViewModel> GetByIdSaveViewModel(int id)
        {
            Entity entity = await _repository.GetByIdAsync(id);

            SaveViewModel viewModel = _mapper.Map<SaveViewModel>(entity);

            return viewModel;
        }

        public virtual async Task<ViewModel> GetByIdViewModel(int id)
        {
            Entity entity = await _repository.GetByIdAsync(id);

            ViewModel viewModel = _mapper.Map<ViewModel>(entity);

            return viewModel;
        }

        public virtual async Task<List<ViewModel>> GetAllViewModel()
        {
            List<Entity> entityList = await _repository.GetAllAsync();

            return _mapper.Map<List<ViewModel>>(entityList);
        }
    }
}
