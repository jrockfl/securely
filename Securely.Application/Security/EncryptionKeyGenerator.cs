using System.Security.Cryptography;

namespace Securely.Application.Security;
public class EncryptionKeyGenerator : IEncryptionKeyGenerator
{
    private readonly int _keySize;

    /// <summary>
    /// Implementation of the IEncryptionKeyGenerator interface.
    /// </summary>
    public EncryptionKeyGenerator(int keySize = 256)
    {
        _keySize = keySize;
    }

    /// <inheritdoc/>
    public string GenerateAesKey()
    {
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        var keyBytes = new byte[_keySize / 8];

        randomNumberGenerator.GetBytes(keyBytes);

        var hexKey = BitConverter.ToString(keyBytes).Replace("-", string.Empty).ToLower();

        return hexKey;
    }
}
