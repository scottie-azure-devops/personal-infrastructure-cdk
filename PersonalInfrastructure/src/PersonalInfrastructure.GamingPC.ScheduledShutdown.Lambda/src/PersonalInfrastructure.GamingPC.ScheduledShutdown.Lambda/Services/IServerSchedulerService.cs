namespace PersonalInfrastructure.GamingPC.ScheduledShutdown.Lambda.Services;

public interface IServerSchedulerService
{
    Task<bool> StopInstances(string targetTagKey, string targetTagValue);
}