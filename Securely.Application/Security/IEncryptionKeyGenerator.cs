namespace Securely.Application.Security;

/// <summary>
/// Interface for generating encryption keys.
/// </summary>
public interface IEncryptionKeyGenerator
{
    /// <summary>
    /// Generates a random AES encryption key.
    /// </summary>
    /// <returns>The generated AES encryption key as a hexadecimal string.</returns>
    string GenerateAesKey();
}
