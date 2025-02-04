using Microsoft.AspNetCore.Mvc;
using Minio.Api.Services;
using System.Collections.Concurrent;

[ApiController]
[Route("api/[controller]")]
public class FileUploadController : ControllerBase
{
    private readonly MinioService _minioService;
    private static readonly ConcurrentDictionary<string, double> _progressTracker = new();

    public FileUploadController(MinioService minioService)
    {
        _minioService = minioService;
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File invalid");

        try
        {
            if (file == null || file.Length == 0)
                return BadRequest("File tidak valid");

            var objectName = $"{Guid.NewGuid()}-{file.FileName}";

            using var stream = file.OpenReadStream();
            await _minioService.UploadFileAsync(
                objectName,
                stream,
                file.Length,
                percentage => Console.WriteLine($"Progress: {percentage}%")
            );

            return Ok(new { objectName });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }
}