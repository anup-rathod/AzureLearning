using BlobUploadandDownload.Models;
using BlobUploadandDownload.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace BlobUploadandDownload.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }
        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> Upload([FromForm] FileModels file)
        {
            await _fileService.Upload(file);
            return Ok("success");
        }
        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> Get(string name)
        {
            var imageFileStream = await _fileService.Get(name);
            string fileType = "jpeg";
            if (name.Contains("png"))
            {
                fileType = "png";
            }
            return File(imageFileStream, $"image/{fileType}");
        }

        [HttpGet]
        [Route("download")]
        public async Task<IActionResult> Download(string name)
        {
            var imageFileStream = await _fileService.Get(name);
            string fileType = "jpeg";
            if (name.Contains("png"))
            {
                fileType = "png";
            }
            return File(imageFileStream, $"image/{fileType}", $"blobfile.{fileType}");
        }

        [HttpGet]
        [Route("all")]
        public IActionResult GetAllNames()
        {
            IEnumerable<string> fileNames = _fileService.GetAllNames();
            return Ok(fileNames);
        }
    }
}
