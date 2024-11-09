using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.FileService;

public class FileService(IWebHostEnvironment hostEnvironment) : IFileService
{
    public async Task<string> SaveFileAsync(IFormFile file)
    {
        var fileName = string.Format($"{Guid.NewGuid() + Path.GetExtension(file.FileName)}");
        var fullPath = Path.Combine(hostEnvironment.WebRootPath, "images", fileName);
        
        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);

        return fileName;
    }

    public bool DeleteFile(string file)
    {
        var fullPath = Path.Combine(hostEnvironment.WebRootPath, "images", file);
        
        File.Delete(fullPath);
        
        return true;
    }
}