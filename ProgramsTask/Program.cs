using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProgramsTask.Processes;
using Microsoft.Extensions.Configuration;
using ProgramsTask.Models;
using ProgramsTask.Helpers.Config;
using ProgramsTask.Interfaces;
using Microsoft.Azure.Cosmos;
using ProgramsTask.Repositories;
using ProgramsTask.Contollers;
using Microsoft.Extensions.Logging;
using Serilog;

class Program
{
    static async Task Main(string[] args)
    {
        await Host.CreateDefaultBuilder(args)
            .ConfigureLogging((hostContext, builder) =>
            {
                builder.ConfigureSerilog(hostContext.Configuration);
            })
            .ConfigureServices((hostContext, services) =>
            {
                IConfiguration configuration = hostContext.Configuration;
                services.AddTransient<consoleProcess>();
                services.AddSingleton<IHostedService, MyConsoleApp>();
                services.AddTransient<applicationFormContoller>();
                services.AddTransient<programController>();
                services.AddTransient<workflowController>();
                services.AddTransient<previewController>();
                services.AddTransient<consoleFeedbackHandler>();
                services.AddTransient<xmlInjectionCheck>();
                services.AddTransient<phoneCheck>();

                string? url = cosmosDBCredentials.URI;
                string primaryKey = cosmosDBCredentials.primaryKey;
                string dbName = cosmosDBCredentials.cosmosDatabase;
                //Addition of Cosmos DB

                services.AddSingleton<IApplicationForm>(options =>
                {
                    string containerName = cosmosDBCredentials.applicationFormContainer;
                    var cosmosClient = new CosmosClient(url, primaryKey);

                    return new applicationForm(cosmosClient, dbName, containerName);
                });
                services.AddSingleton<IProgram>(options =>
                {
                    string containerName = cosmosDBCredentials.programContainer;
                    var cosmosClient = new CosmosClient(
                        url,
                        primaryKey
                    );

                    return new programme(cosmosClient, dbName, containerName);
                });
                services.AddSingleton<IWorkflow>(options =>
                {
                    string containerName = cosmosDBCredentials.workflowContainer;
                    var cosmosClient = new CosmosClient(
                        url,
                        primaryKey
                    );

                    return new workFlow(cosmosClient, dbName, containerName);
                });
                services.AddSingleton<IPreview>(options =>
                {
                    string containerName = cosmosDBCredentials.previewContainer;
                    var cosmosClient = new CosmosClient(
                        url,
                        primaryKey
                    );

                    return new preview(cosmosClient, dbName, containerName);
                });
            })
            .RunConsoleAsync();



        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddDebug();
            builder.AddEventLog();
            builder.AddConsole();
        });

        var logger = loggerFactory.CreateLogger<Program>();
    }
}

public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder ConfigureSerilog(this ILoggingBuilder loggingBuilder, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        return loggingBuilder;
    }
}
