using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.FileService;

public interface IFileService
{
    public Task<string> SaveFileAsync(string path, IFormFile file);
    public bool DeleteFile(string file);
}