using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace expensetracker.api.Application.Common;

public static class ETagHelper
{
    public static string GenerateETag(object resource)
    {
        var json = JsonSerializer.Serialize(resource);
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(json));
        return Convert.ToBase64String(hash);
    }
}