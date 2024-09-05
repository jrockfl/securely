using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Securely.Application.Security;

namespace PwC.Securely.Tests.Application.Security;

[TestClass]
public class EncryptionKeyGeneratorTests
{
    [TestMethod]
    public void GenerateAesKey_WhenExecuted_GeneratesEncryptionKey()
    {
        // Arrange
        var encryptionKeyGenerator = new EncryptionKeyGenerator();

        // Act
        var encryptionKey = encryptionKeyGenerator.GenerateAesKey();

        // Assert 
        encryptionKey.Should().NotBeNull();
    }

    [TestMethod]
    public void GenerateAesKey_WhenExecuted_HasCorrectLength()
    {
        // Arrange
        var encryptionKeyGenerator = new EncryptionKeyGenerator();

        // Act
        var encryptionKey = encryptionKeyGenerator.GenerateAesKey();
        var keyBytes = StringToByteArray(encryptionKey);

        // Assert 
        keyBytes.Length.Should().Be(32);
    }

    // Helper method to convert a hexadecimal string to a byte array
    private byte[] StringToByteArray(string hex)
    {
        var length = hex.Length;
        var bytes = new byte[length / 2];
        for (var i = 0; i < length; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }
        return bytes;
    }
}
