using BlobUploadandDownload.Models;
using System.IO;

namespace BlobUploadandDownload.Services
{
    public interface IFileService
    {
        Task<Stream> Get(string name);
        Task Upload(FileModels fileModel);
        public IEnumerable<string> GetAllNames();
    }
}
