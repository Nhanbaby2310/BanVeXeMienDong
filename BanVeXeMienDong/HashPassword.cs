// Tool để hash mật khẩu - Sử dụng BCrypt (an toàn, có salt tự động)
// Chạy: dotnet run -- hash <password>
class PasswordHasher
{
    static void Main(string[] args)
    {
        string password = args.Length > 0 ? args[0] : "quanly";

        // BCrypt tự động tạo salt ngẫu nhiên, workFactor = 12 (mặc định)
        string hash = BCrypt.Net.BCrypt.HashPassword(password);

        Console.WriteLine($"Password: {password}");
        Console.WriteLine($"BCrypt Hash: {hash}");
        Console.WriteLine();
        Console.WriteLine("// Verify test:");
        Console.WriteLine($"Verify result: {BCrypt.Net.BCrypt.Verify(password, hash)}");
    }
}
