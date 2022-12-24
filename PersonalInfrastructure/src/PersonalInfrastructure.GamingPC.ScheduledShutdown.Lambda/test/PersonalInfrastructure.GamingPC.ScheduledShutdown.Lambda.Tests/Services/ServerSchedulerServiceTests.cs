using FakeItEasy;
using FluentAssertions;
using PersonalInfrastructure.GamingPC.ScheduledShutdown.Lambda.Providers;
using PersonalInfrastructure.GamingPC.ScheduledShutdown.Lambda.Services;
using Xunit;

namespace PersonalInfrastructure.GamingPC.ScheduledShutdown.Lambda.Tests.Services;

public class ServerSchedulerServiceTests
{
    [Fact]
    public async Task Should_StopInstances_ForStandardInput()
    {
        // arrange
        string targetTagKey = "NightlyStop";
        string targetTagValue = "True";
        List<string> instanceIds = new List<string>() { "i-1234567890abcdef0" };
        IEc2Provider ec2Provider = A.Fake<IEc2Provider>();
        A.CallTo(() => ec2Provider.GetInstancesForTag(targetTagKey, targetTagValue)).Returns(instanceIds);
        A.CallTo(() => ec2Provider.StopInstances(instanceIds)).Returns(true);
        IServerSchedulerService serverSchedulerService = new ServerSchedulerService(ec2Provider);

        // act
        bool response = await serverSchedulerService.StopInstances(targetTagKey, targetTagValue);
        
        // assert
        A.CallTo(() => ec2Provider.GetInstancesForTag(targetTagKey, targetTagValue)).MustHaveHappened();
        A.CallTo(() => ec2Provider.StopInstances(instanceIds)).MustHaveHappened();
        response.Should().BeTrue();
    }
}