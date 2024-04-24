namespace RoyalState.Core.Application.Interfaces.Services
{
    public interface IGenericService<SaveViewModel, ViewModel, Entity>
        where SaveViewModel : class
        where ViewModel : class
        where Entity : class
    {
        Task<SaveViewModel> Add(SaveViewModel viewModel);
        Task Update(SaveViewModel viewModel, int id);
        Task Delete(int id);
        Task<SaveViewModel> GetByIdSaveViewModel(int id);
        Task<ViewModel> GetByIdViewModel(int id);
        Task<List<ViewModel>> GetAllViewModel();
    }
}
