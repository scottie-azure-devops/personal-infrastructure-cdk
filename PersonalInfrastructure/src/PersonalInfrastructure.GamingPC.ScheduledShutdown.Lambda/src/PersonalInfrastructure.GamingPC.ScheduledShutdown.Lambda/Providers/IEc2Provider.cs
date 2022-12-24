namespace PersonalInfrastructure.GamingPC.ScheduledShutdown.Lambda.Providers;

public interface IEc2Provider
{
    Task<List<string>> GetInstancesForTag(string targetTagKey, string targetTagValue);
    Task<bool> StopInstances(List<string> instanceIds);
}