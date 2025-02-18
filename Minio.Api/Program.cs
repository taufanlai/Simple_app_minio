using Microsoft.AspNetCore.Http.Features;
using Minio.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 12L * 1024 * 1024 * 1024; // 12GB (lebih dari 11GB)
});

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 12L * 1024 * 1024 * 1024; // 12GB
});


// Tambahkan konfigurasi Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "MinIO Upload API", Version = "v1" });
});

// CORS Configuration
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowWebApp",
//         policy => policy.WithOrigins("https://localhost:7175")
//                         .AllowAnyMethod()
//                         .AllowAnyHeader());
// });

// Registrasi service
builder.Services.AddControllers();
builder.Services.AddSingleton<MinioService>();

var app = builder.Build();

// Aktifkan Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MinIO API v1");
});

// Middleware
app.UseCors("AllowWebApp");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();