using System.Security.Cryptography;
using System.Text;

// Tool để hash mật khẩu
class PasswordHasher
{
    static void Main()
    {
        string password = "quanly";
        
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha.ComputeHash(bytes);
        var hashString = Convert.ToBase64String(hash);
        
        Console.WriteLine($"Password: {password}");
        Console.WriteLine($"Hash: {hashString}");
    }
}
