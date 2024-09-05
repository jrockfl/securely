using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using Securely.Application.Helpers;
using Securely.Infrastructure.ServiceBus;

namespace Securely.Application.OptionsSetup;

public class ServiceBusOptionsSetup : IConfigureOptions<ServiceBusOptions>
{
    private const string SectionName = "ServiceBus";
    private readonly IConfiguration _configuration;

    public ServiceBusOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(ServiceBusOptions options)
    {
        Argument.AssertNotNull(options, nameof(options));
        _configuration.GetSection(SectionName).Bind(options);
    }
}
