using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using Securely.Application.Helpers;
using Securely.Infrastructure.KeyVault;

namespace Securely.Application.OptionsSetup;

public class KeyVaultOptionsSetup : IConfigureOptions<KeyVaultOptions>
{
    private const string SectionName = "KeyVault";
    private readonly IConfiguration _configuration;

    public KeyVaultOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(KeyVaultOptions options)
    {
        Argument.AssertNotNull(options, nameof(options));
        _configuration.GetSection(SectionName).Bind(options);
    }
}
