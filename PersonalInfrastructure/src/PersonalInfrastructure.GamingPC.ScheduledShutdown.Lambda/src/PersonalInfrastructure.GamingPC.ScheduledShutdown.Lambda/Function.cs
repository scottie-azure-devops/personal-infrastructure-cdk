using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;
using PersonalInfrastructure.GamingPC.ScheduledShutdown.Lambda.Providers;
using PersonalInfrastructure.GamingPC.ScheduledShutdown.Lambda.Services;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace PersonalInfrastructure.GamingPC.ScheduledShutdown.Lambda;

public class Function
{
    private static IServiceProvider _serviceProvider;
    private readonly IServerSchedulerService _serverSchedulerService;

    public Function()
    {
        ServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<IEc2Provider, Ec2Provider>();
        serviceCollection.AddTransient<IServerSchedulerService, ServerSchedulerService>();
        _serviceProvider = serviceCollection.BuildServiceProvider();
        _serverSchedulerService = _serviceProvider.GetRequiredService<IServerSchedulerService>();
    }
    
    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="context">Lambda context</param>
    /// <returns></returns>
    public async Task<bool> FunctionHandler(ILambdaContext context)
    {
        return await _serverSchedulerService.StopInstances("NightlyShutdown", "True");
    }
}
