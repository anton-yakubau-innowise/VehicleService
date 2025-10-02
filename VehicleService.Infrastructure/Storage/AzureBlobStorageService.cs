using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VehicleService.Application.Interfaces;

namespace VehicleService.Infrastructure.Storage
{
    public class AzureBlobStorageService(IOptions<StorageSettings> storageSettings, ILogger<AzureBlobStorageService> logger) : IFileStorageService
    {
        private readonly StorageSettings _storageSettings = storageSettings.Value;
        private readonly BlobServiceClient _blobServiceClient = new(storageSettings.Value.ConnectionString);

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
            try
            {
                var containerClient = await GetContainerClientAsync();

                var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";

                var blobClient = containerClient.GetBlobClient(uniqueFileName);

                logger.LogInformation("Uploading file {FileName} to Azure Blob Storage as {UniqueFileName}", fileName, uniqueFileName);

                await blobClient.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType });

                logger.LogInformation("File {UniqueFileName} uploaded successfully. URL: {BlobUrl}", uniqueFileName, blobClient.Uri.ToString());

                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while uploading file {FileName} to Azure Blob Storage.", fileName);
                throw;
            }
        }

        private async Task<BlobContainerClient> GetContainerClientAsync()
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_storageSettings.ContainerName);

            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
            
            logger.LogDebug("Blob container '{ContainerName}' is ready.", _storageSettings.ContainerName);
            
            return containerClient;
        }
    }
}

