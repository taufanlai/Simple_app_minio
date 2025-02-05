using Microsoft.AspNetCore.Mvc;
using Minio.Api.Services;
using System.Collections.Concurrent;

[ApiController]
[Route("api/[controller]")]
public class DeleteController : ControllerBase
{
    private readonly MinioService _minioService;

    public DeleteController(MinioService minioService)
    {
        _minioService = minioService;
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteFile([FromQuery] string fileName)
    {
        try
        {
            await _minioService.DeleteFileAsync(fileName);
            return Ok($"File {fileName} deleted successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }
}