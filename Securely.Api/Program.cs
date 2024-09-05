using System.Text.Json;

using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.EntityFrameworkCore;

using Securely.Application.Commands;
using Securely.Application.Events;
using Securely.Application.OptionsSetup;
using Securely.Application.Queries;
using Securely.Application.Security;
using Securely.Infrastructure.KeyVault;
using Securely.Infrastructure.Repositories;
using Securely.Infrastructure.ServiceBus;


namespace Securely.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

        var aiOptions = builder.Configuration.GetSection("ApplicationInsights").Get<ApplicationInsightsServiceOptions>();
        builder.Services.AddApplicationInsightsTelemetry(aiOptions);

        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddConsole();
            loggingBuilder.AddDebug();
            loggingBuilder.AddApplicationInsights();
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<SecurelyDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<IGenerateEncryptionKeyCommand, GenerateEncryptionKeyCommand>();
        builder.Services.AddScoped<IGetEncryptionKeyByIdQuery, GetEncryptionKeyByIdQuery>();
        builder.Services.AddScoped<IKeyVaultManager, AzureKeyVaultManager>();
        builder.Services.AddScoped<ICommandRepository, CommandRepository>();
        builder.Services.AddScoped<IQueryRepository, QueryRepository>();
        builder.Services.AddScoped<IEncryptionKeyGenerator, EncryptionKeyGenerator>();
        builder.Services.AddScoped<ICreateSecureMessageCommand, CreateSecureMessageCommand>();
        builder.Services.AddScoped<ICreateSecureMessageHistoryCommand, CreateSecureMessageHistoryCommand>();
        builder.Services.AddScoped<IGetSecureMessageByIdQuery, GetSecureMessageByIdQuery>();
        builder.Services.AddScoped<IGetEncryptionKeyByMessgeIdQuery, GetEncryptionKeyByMessgeIdQuery>();
        builder.Services.AddScoped<IUpdateSecureMessageCommand, UpdateSecureMessageCommand>();
        builder.Services.AddScoped<IDeleteSecureMessageCommand, DeleteSecureMessageCommand>();
        builder.Services.AddScoped<IDeleteEncryptionKeyCommand, DeleteEncryptionKeyCommand>();
        builder.Services.AddScoped<IDeleteSecureMesssageAndEncryptionKeyCommand, DeleteSecureMesssageAndEncryptionKeyCommand>();
        builder.Services.AddScoped<IServiceBusManager, AzureServiceBusManager>();
        builder.Services.AddScoped<ISendEventToServiceBusDispatcher, SendEventToServiceBusDispatcher>();

        builder.Services.AddProblemDetails();
        builder.Services.AddOptions();
        builder.Services.ConfigureOptions<KeyVaultOptionsSetup>();
        builder.Services.ConfigureOptions<ServiceBusOptionsSetup>();
        builder.Services.AddCors();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
