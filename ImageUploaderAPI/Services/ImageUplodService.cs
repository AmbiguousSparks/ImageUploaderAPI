using ImageUploaderAPI.Models.Enums;
using ImageUploaderAPI.Models.Output;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ImageUploaderAPI.Services
{
    public class ImageUplodService : IImageUploadService
    {
        private readonly string fileDirectory = $"{AppContext.BaseDirectory}\\files\\";
        public async Task<byte[]> GetImage(string fileName, CancellationToken cancellationToken = default)
        {
            byte[] bytes = null;
            using (var fileStream = new FileStream(fileDirectory + fileName, FileMode.Open))
            {
                bytes = new byte[fileStream.Length];
                await fileStream.ReadAsync(bytes, cancellationToken);
            }
            return bytes;
        }

        public async Task<OutImageUpload> ReadImage(IFormFile formFile, CancellationToken cancellationToken = default)
        {
            try
            {
                ImageMimeTypes fileExt = Enum.Parse<ImageMimeTypes>(formFile.FileName.Split('.').Last());
                if (fileExt == ImageMimeTypes.undefined)
                    throw new ArgumentException("File must be an image!");
                if (!Directory.Exists(fileDirectory))
                    Directory.CreateDirectory(fileDirectory);
                string fileName = Guid.NewGuid() + formFile.FileName;
                OutImageUpload response = new()
                {
                    FileName = fileName
                };
                using (var fileStream = new FileStream(fileDirectory + fileName, FileMode.OpenOrCreate))
                {
                    await formFile.CopyToAsync(fileStream, cancellationToken);
                }
                return response;
            }catch(ArgumentException)
            {
                throw new ArgumentException("File must be an image!");
            }
        }

        public async Task<ICollection<OutImageUpload>> ReadImages(IFormFileCollection formFiles, CancellationToken cancellationToken = default)
        {
            ICollection<OutImageUpload> response = new List<OutImageUpload>(formFiles.Count);
            foreach (IFormFile formFile in formFiles)
            {
                response.Add(await ReadImage(formFile, cancellationToken));
            }
            return response;
        }
    }
}
