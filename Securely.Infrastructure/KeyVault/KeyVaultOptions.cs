namespace Securely.Infrastructure.KeyVault;
public class KeyVaultOptions
{
    public string TenantId { get; set; }

    public string ClientId { get; set; }

    public string ClientSecret { get; set; }

    public string KeyVaultName { get; set; }
}
