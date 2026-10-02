using System.Security.Cryptography;
namespace Example.Web.Services;
public sealed class ProductCodeGenerator : IProductCodeGenerator
{
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    public string Generate()
    {
        Span<char> code = stackalloc char[35];
        for (var i = 0; i < code.Length; i++)
            code[i] = (i + 1) % 6 == 0 ? '-' : Alphabet[RandomNumberGenerator.GetInt32(Alphabet.Length)];
        return new string(code);
    }
}

