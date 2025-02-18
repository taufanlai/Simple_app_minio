using Microsoft.AspNetCore.Mvc;
using Minio.Api.Services;
using System.Collections.Concurrent;

[ApiController]
[Route("api/[controller]")]
public class Download2Controller : ControllerBase
{
    private readonly MinioService _minioService;

    public Download2Controller(MinioService minioService)
    {
        _minioService = minioService;
    }

    [HttpGet]
    public async Task DownloadFile([FromQuery] string fileName)
    {
        try
        {
            var res = await _minioService.DownloadFile2Async(fileName);

            // Set header untuk download
          //  Response.Headers.Append("Content-Disposition", $"attachment; filename=\"{fileName}\"");
            //return File(fileStream, "application/octet-stream");

                 Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                Response.ContentType = res.ContentType;
                await res.Stream.CopyToAsync(Response.Body);

        }
        catch (Exception ex)
        {
            //return StatusCode(500, $"Error: {ex.Message}");
        }
    }



}