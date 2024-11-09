namespace Infrastructure.Services.HashService;

public interface IHashService
{
    public string ConvertToHash(string rawData);
    public bool VerifyHash(string rawData, string expectedHash);
}