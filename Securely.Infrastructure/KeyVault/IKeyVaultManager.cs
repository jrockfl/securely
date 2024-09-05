namespace Securely.Infrastructure.KeyVault;

public interface IKeyVaultManager
{
    Task SaveKeyAsync(string id, string key);

    Task<string> GetKeyAsync(string id);

    Task DeleteKeyAsync(string id);
}
