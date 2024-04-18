using Microsoft.AspNetCore.Http;

namespace RoyalState.Core.Application.Interfaces.Services
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string email, bool isEditMode = false, string imagePath = "");
        Task DeleteFileAsync(string imagePath);
    }
}
