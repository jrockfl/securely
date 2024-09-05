using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Securely.Infrastructure.KeyVault;

public class AzureKeyVaultManager : IKeyVaultManager
{
    private readonly string _keyVaultUri;
    private readonly SecretClient _secretClient;
    private readonly IOptions<KeyVaultOptions> _options;
    private readonly ILogger<AzureKeyVaultManager> _logger;

    public AzureKeyVaultManager(IOptions<KeyVaultOptions> options, ILogger<AzureKeyVaultManager> logger)
    {
        _options = options;
        _keyVaultUri = $"https://{_options.Value.KeyVaultName}.vault.azure.net";
        _secretClient = new SecretClient(new Uri(_keyVaultUri), new ClientSecretCredential(_options.Value.TenantId, _options.Value.ClientId, _options.Value.ClientSecret));
        _logger = logger;
    }

    public async Task DeleteKeyAsync(string id)
    {
        try
        {
            _logger.LogInformation($"Deleting key {id} from the key vault {_options.Value.KeyVaultName}.");
            await _secretClient.StartDeleteSecretAsync(id);
            _logger.LogInformation($"Deleted key {id} from the key vault {_options.Value.KeyVaultName} successfully.");
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            _logger.LogWarning($"Key {id} not found in the key vault {_options.Value.KeyVaultName}.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An unexpected error occurred while deleting key {id} from the key vault {_options.Value.KeyVaultName}.", ex);
            throw;
        }
    }

    public async Task<string> GetKeyAsync(string id)
    {
        try
        {
            _logger.LogInformation($"Retrieving key {id} from the key vault {_options.Value.KeyVaultName}.");
            var key = await _secretClient.GetSecretAsync(id);
            _logger.LogInformation($"Retrieved key {id} from the key vault {_options.Value.KeyVaultName}. successfully.");
            return key.Value.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError($"An unexpected error occurred while retrieving key {id} from the key vault {_options.Value.KeyVaultName}.", ex);
            throw;
        }
    }

    public async Task SaveKeyAsync(string id, string key)
    {
        try
        {
            _logger.LogInformation($"Saving key {id} to the key vault {_options.Value.KeyVaultName}.");
            await _secretClient.SetSecretAsync(id, key);
            _logger.LogInformation($"Saved key {id} to the key vault {_options.Value.KeyVaultName} successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"An unexpected error occurred while saving key {id} to the key vault {_options.Value.KeyVaultName}.", ex);
            throw;
        }
    }
}
