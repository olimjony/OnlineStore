using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services.HashService;

public class HashService() : IHashService
{
    public string ConvertToHash(string rawData)
    {
        using var sha256Hash = SHA256.Create();
        var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

        var builder = new StringBuilder();
        foreach (var t in bytes)
        {
            builder.Append(t.ToString("x2"));
        }
        return builder.ToString();
    }

    public bool VerifyHash(string rawData, string expectedHash)
    {
        var hashOfInput = ConvertToHash(rawData);

        return StringComparer.OrdinalIgnoreCase.Compare(hashOfInput, expectedHash) == 0;
    }
}