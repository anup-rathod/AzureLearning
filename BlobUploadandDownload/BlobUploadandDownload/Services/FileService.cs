using System.IO;

using Azure.Storage.Blobs;
using BlobUploadandDownload.Models;

namespace BlobUploadandDownload.Services
{
    public class FileService : IFileService
    {
        private readonly BlobServiceClient _blobServiceClient;
        public FileService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task Upload(FileModels fileModel)
        {
            var containerInstance = _blobServiceClient.GetBlobContainerClient("mainlearncontainer");

            var blobInstance = containerInstance.GetBlobClient(fileModel.ImageFile.FileName);

            await blobInstance.UploadAsync(fileModel.ImageFile.OpenReadStream());
        }

        public async Task<Stream> Get(string name)
        {
            var containerInstance = _blobServiceClient.GetBlobContainerClient("mainlearncontainer");

            var blobInstance = containerInstance.GetBlobClient(name);

            var downloadContent = await blobInstance.DownloadAsync();

            return downloadContent.Value.Content;

        }

        public IEnumerable<string> GetAllNames()
        {
            var containerInstance = _blobServiceClient.GetBlobContainerClient("mainlearncontainer");
            var blobItems = containerInstance.GetBlobs();
            List<string> blobNames = new List<string>();
            foreach (var blobItem in blobItems)
            {
                blobNames.Add(blobItem.Name);
            }
            return blobNames;
        }
    }
}
