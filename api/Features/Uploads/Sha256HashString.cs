using System.Security.Cryptography;
using Vogen;

namespace Koworking.Api.Features.Uploads;

[ValueObject<string>(Conversions.SystemTextJson | Conversions.EfCoreValueConverter)]
public readonly partial struct Sha256HashString
{
    public const int LengthBytes = SHA256.HashSizeInBytes;
    public const int LengthChars = LengthBytes * 2;

    private static string NormalizeInput(string input) => input.ToUpper();

    private static Validation Validate(string input) => input switch
    {
        { Length: not LengthChars } => Validation.Invalid($"{nameof(Sha256HashString)} length must be {LengthChars}."),
        _ when input.All(char.IsAsciiHexDigitUpper) => Validation.Ok,
        _ => Validation.Invalid($"input string {input} contains non-hex characters")
    };

    public static Sha256HashString FromBytes(ReadOnlySpan<byte> bytes) => bytes.Length != LengthBytes
        ? throw new ArgumentException($"{nameof(Sha256HashString)} must have exactly {LengthBytes} bytes.")
        : From(Convert.ToHexString(bytes));

    public static async ValueTask<Sha256HashString> CalculateAsync(Stream data, CancellationToken ct)
        => FromBytes(await SHA256.HashDataAsync(data, ct));

}