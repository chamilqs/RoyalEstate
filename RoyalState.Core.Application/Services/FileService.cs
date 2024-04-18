using Microsoft.AspNetCore.Http;
using RoyalState.Core.Application.Interfaces.Services;

namespace RoyalState.Core.Application.Services
{
    public class FileService : IFileService
    {
        #region UploadFile
        /// <summary>
        /// Uploads a file asynchronously locally.
        /// </summary>
        /// <param name="file">The file to upload.</param>
        /// <param name="email">The email of the user.</param>
        /// <param name="isEditMode">Indicates whether it is in edit mode.</param>
        /// <param name="imagePath">The path of the image.</param>
        /// <returns>The path of the uploaded file.</returns>
        public async Task<string> UploadFileAsync(IFormFile file, string email, bool isEditMode = false, string imagePath = "")
        {
            if (isEditMode && file == null)
            {
                return imagePath;
            }

            string basePath = $"/Images/Users/{email}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            Guid guid = Guid.NewGuid();
            string fileName = $"{guid}{Path.GetExtension(file.FileName)}";
            string fileNameWithPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            if (isEditMode)
            {
                string[] oldImagePart = imagePath.Split("/");
                string oldImagePath = oldImagePart[^1];
                string completeImageOldPath = Path.Combine(path, oldImagePath);

                if (File.Exists(completeImageOldPath))
                {
                    File.Delete(completeImageOldPath);
                }
            }

            return $"{basePath}/{fileName}";
        }
        #endregion

        #region DeleteFile
        /// <summary>
        /// Deletes a file asynchronously locally.
        /// </summary>
        /// <param name="imagePath">The path of the image.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteFileAsync(string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
                return;

            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath.TrimStart('/'));

            if (File.Exists(fullPath))
            {
                try
                {
                    await Task.Run(() => File.Delete(fullPath));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"an error ocured while trying to delete the file '{fullPath}': {ex.Message}");
                }
            }
        }
        #endregion

    }
}
