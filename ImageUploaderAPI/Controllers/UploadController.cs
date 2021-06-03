using ImageUploaderAPI.Extensions;
using ImageUploaderAPI.Models;
using ImageUploaderAPI.Models.Enums;
using ImageUploaderAPI.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ImageUploaderAPI.Controllers
{
    [EnableCors("DefaultPolicy"), Route("/api/Upload")]
    public class UploadController : Controller
    {
        private readonly IImageUploadService _imageUploadService;
        public UploadController(IImageUploadService imageUploadService)
        {
            _imageUploadService = imageUploadService;
        }
        /// <summary>
        /// Faz o upload de uma imagem
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("/UploadImages"), 
            ProducesResponseType(StatusCodes.Status200OK), 
            ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadImages(CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _imageUploadService.ReadImages(Request.Form.Files, cancellationToken);
                return Ok(response);
            }catch(ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet, Route("/GetImage")]
        public async Task<IActionResult> GetImage([FromQuery] InImageUpload inImageUpload, CancellationToken cancellationToken = default)
        {
            var response = await _imageUploadService.GetImage(inImageUpload.FileName, cancellationToken);

            ImageMimeTypes fileExt = Enum.Parse<ImageMimeTypes>(inImageUpload.FileName.Split('.').Last());
            return File(response, fileExt.Description());
        }
    }
}
