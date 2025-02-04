using System.Net;

public class ProgressableStreamContent : HttpContent
{
    private readonly HttpContent _content;
    private readonly IProgress<double> _progress;

    public ProgressableStreamContent(HttpContent content, IProgress<double> progress)
    {
        _content = content;
        _progress = progress;
    }

    protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
    {
        var buffer = new byte[8192];
        using var contentStream = await _content.ReadAsStreamAsync();
        long totalBytes = contentStream.Length;
        long uploadedBytes = 0;

        int read;
        while ((read = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            await stream.WriteAsync(buffer, 0, read);
            await stream.FlushAsync(); // ✅ Tambahkan FlushAsync untuk memastikan streaming real-time
            uploadedBytes += read;
            _progress.Report((uploadedBytes / (double)totalBytes) * 100);
        }
    }

    protected override bool TryComputeLength(out long length)
    {
        length = _content.Headers.ContentLength ?? -1;
        return length != -1;
    }
}
