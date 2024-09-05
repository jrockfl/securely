using System.Reflection;

using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Securely.Application.Handlers;
using Securely.Application.OptionsSetup;
using Securely.Infrastructure.KeyVault;
using Securely.Infrastructure.Repositories;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddDbContext<SecurelyDbContext>(options =>
            options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection"))
        );

        services.AddScoped<IKeyVaultManager, AzureKeyVaultManager>();
        services.AddScoped<EventHandlerFactory, EventHandlerFactory>();       

        services.Scan(scan => scan
            .FromAssemblies(
                Assembly.GetExecutingAssembly(),
                Assembly.Load(Securely.Application.AssemblyReference.Assembly.GetName()),
                Assembly.Load(Securely.Infrastructure.AssemblyReference.Assembly.GetName())
            )
            .AddClasses(classes => classes.InNamespaces("Securely.Application.Commands"))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.InNamespaces("Securely.Application.Queries"))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.InNamespaces("Securely.Application.Handlers"))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.InNamespaces("Securely.Infrastructure.Repositories"))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.InNamespaces("Securely.Infrastructure.ServiceBus"))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );

        services.AddOptions();
        services.ConfigureOptions<KeyVaultOptionsSetup>();

        services.AddLogging();
    })
    .Build();

host.Run();
