namespace VehicleService.Infrastructure.Storage
{
    public class StorageSettings
    {
        public const string SectionName = "AzureBlobStorage";

        public string ConnectionString { get; init; } = string.Empty;
        public string ContainerName { get; init; } = string.Empty;
    }
}
