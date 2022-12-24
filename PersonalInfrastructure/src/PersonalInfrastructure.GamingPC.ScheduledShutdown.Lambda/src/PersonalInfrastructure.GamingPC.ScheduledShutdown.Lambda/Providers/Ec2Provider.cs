using Amazon.EC2;
using Amazon.EC2.Model;

namespace PersonalInfrastructure.GamingPC.ScheduledShutdown.Lambda.Providers;

public class Ec2Provider : IEc2Provider
{
    private readonly AmazonEC2Client _ec2Client; 
    
    public Ec2Provider()
    {
        _ec2Client = new AmazonEC2Client();
    }

    public async Task<List<string>> GetInstancesForTag(string targetTagKey, string targetTagValue)
    {
        List<string> instanceIds = new List<string>();
        DescribeInstancesResponse describeInstancesResponse = await _ec2Client.DescribeInstancesAsync(new DescribeInstancesRequest()
        {
            Filters = new List<Filter>()
            {
                new Filter()
                {
                    Name = $"tag:{targetTagKey}",
                    Values = new List<string>() { targetTagValue }
                }
            }
        });
        foreach (Reservation reservation in describeInstancesResponse.Reservations)
        {
            foreach (var instance in reservation.Instances)
            {
               instanceIds.Add(instance.InstanceId); 
            }
        }
        return instanceIds;
    }

    public async Task<bool> StopInstances(List<string> instanceIds)
    {
        StopInstancesResponse stopInstancesResponse = await _ec2Client.StopInstancesAsync(new StopInstancesRequest()
        {
            InstanceIds = instanceIds
        });
        return true;
    }
}