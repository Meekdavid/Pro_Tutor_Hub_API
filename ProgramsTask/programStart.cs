using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProgramsTask.Helpers.Config;
using ProgramsTask.Processes;
using ProgramsTask.Models;
using System.Configuration;

public class MyConsoleApp : IHostedService
{   

    public IConfiguration Configuration { get; }
    private readonly IServiceScopeFactory _scopeFactory;

    public MyConsoleApp(IServiceScopeFactory scopeFactory, IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        Configuration = configuration;
        ConfigurationSettingsHelper.Configuration = configuration;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();

        // Do something with the services in the scope
        // For example, you can resolve a service and call its methods
        var myService = scope.ServiceProvider.GetRequiredService<consoleProcess>();
        myService.API();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
