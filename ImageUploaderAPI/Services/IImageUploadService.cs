using ImageUploaderAPI.Models.Output;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ImageUploaderAPI.Services
{
    public interface IImageUploadService
    {
        Task<OutImageUpload> ReadImage(IFormFile formFile, CancellationToken cancellationToken = default);
        Task<ICollection<OutImageUpload>> ReadImages(IFormFileCollection formFiles, CancellationToken cancellationToken = default);
        Task<byte[]> GetImage(string fileName, CancellationToken cancellationToken = default);
    }
}
