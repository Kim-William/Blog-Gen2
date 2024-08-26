using System.Net;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Wkkim.Blog.Web.Repositories;

namespace Wkkim.Blog.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file was provided.");
            }

            try
            {
                var imageUrl = await imageRepository.UploadAsync(file);
                return Ok(new { link = imageUrl }); // JSON 형식의 응답 반환
            }
            catch (Exception ex)
            {
                // 서버에서 오류가 발생하면 명확한 오류 메시지와 상태 코드 반환
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}