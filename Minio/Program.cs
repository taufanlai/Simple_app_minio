using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Minio.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new ProducesAttribute("application/json"));
});

builder.Services.AddSignalR();
builder.Services.AddScoped<FileUploadService>(); // Tambahkan ini 🟡
builder.Services.AddHttpClient(); // 🟡 Untuk HttpClient

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 12L * 1024 * 1024 * 1024; // 12GB (lebih dari 11GB)
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.MapHub<UploadHub>("/uploadHub");
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
