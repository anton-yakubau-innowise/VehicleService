using System.IO;
using System.Threading.Tasks;

namespace VehicleService.Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType);
    }
}
