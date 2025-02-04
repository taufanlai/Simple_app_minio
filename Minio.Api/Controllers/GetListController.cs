using Microsoft.AspNetCore.Mvc;
using Minio.Api.Services;
using System.Collections.Concurrent;

[ApiController]
[Route("api/[controller]")]
public class GetListController : ControllerBase
{
    private readonly MinioService _minioService;
    private static readonly ConcurrentDictionary<string, double> _progressTracker = new();

    public GetListController(MinioService minioService)
    {
        _minioService = minioService;
    }

    [HttpGet]
    public async Task<IActionResult> GetListData(string bucket)
    {
        try
        {
            // Panggil metode service yang mengembalikan daftar file dengan informasi lengkap
            var fileList = await _minioService.GetFileListAsync(bucket);

            // Kirim hasil sebagai response
            return Ok(fileList);
        }
        catch (Exception ex)
        {
            // Tangani error dan kembalikan status 500 jika ada masalah
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }
}