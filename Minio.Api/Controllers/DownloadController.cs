using Microsoft.AspNetCore.Mvc;
using Minio.Api.Services;
using System.Collections.Concurrent;

[ApiController]
[Route("api/[controller]")]
public class DownloadController : ControllerBase
{
    private readonly MinioService _minioService;

    public DownloadController(MinioService minioService)
    {
        _minioService = minioService;
    }

    [HttpGet]
    public async Task<IActionResult> DownloadFile([FromQuery] string bucket, [FromQuery] string fileName)
    {
        try
        {
            var fileStream = await _minioService.DownloadFileAsync(bucket, fileName);

            // Set header untuk download
            Response.Headers.Append("Content-Disposition", $"attachment; filename=\"{fileName}\"");
            return File(fileStream, "application/octet-stream");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }



}