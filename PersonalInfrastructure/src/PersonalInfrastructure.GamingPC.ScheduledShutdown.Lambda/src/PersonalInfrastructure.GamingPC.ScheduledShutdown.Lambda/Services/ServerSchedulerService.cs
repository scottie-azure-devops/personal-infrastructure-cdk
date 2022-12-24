using PersonalInfrastructure.GamingPC.ScheduledShutdown.Lambda.Providers;

namespace PersonalInfrastructure.GamingPC.ScheduledShutdown.Lambda.Services;

public class ServerSchedulerService : IServerSchedulerService
{
    private readonly IEc2Provider _ec2Provider;
    
    public ServerSchedulerService(IEc2Provider ec2Provider)
    {
        _ec2Provider = ec2Provider;
    }
    
    public async Task<bool> StopInstances(string targetTagKey, string targetTagValue)
    {
        List<string> targetInstanceIds = await _ec2Provider.GetInstancesForTag(targetTagKey, targetTagValue);
        return await _ec2Provider.StopInstances(targetInstanceIds);
    }
}