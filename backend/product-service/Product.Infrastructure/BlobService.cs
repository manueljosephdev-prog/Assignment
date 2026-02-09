
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace Product.Infrastructure;

public class BlobService
{
    private readonly BlobContainerClient _container;

    public BlobService(IConfiguration config)
    {
        var conn = config["AzureBlob:ConnectionString"];
        var container = config["AzureBlob:ContainerName"];

        var client = new BlobServiceClient(conn);
        _container = client.GetBlobContainerClient(container);
        _container.CreateIfNotExists(PublicAccessType.Blob);
    }

    public async Task<(string Url, string BlobName)> UploadAsync(IFormFile file)
    {
        var blobName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var blob = _container.GetBlobClient(blobName);

        using var stream = file.OpenReadStream();
        await blob.UploadAsync(stream, true);

        return (blob.Uri.ToString(), blobName);
    }

    public async Task DeleteAsync(string blobName)
    {
        var blob = _container.GetBlobClient(blobName);
        await blob.DeleteIfExistsAsync();
    }
}
