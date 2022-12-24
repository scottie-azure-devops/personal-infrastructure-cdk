using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Events;
using Amazon.CDK.AWS.Events.Targets;
using Constructs;

namespace PersonalInfrastructure
{
    public class PersonalInfrastructureStack : Stack
    {
        internal PersonalInfrastructureStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            // Lambda function
            DockerImageCode gamingPcShutdownLambdaDockerImageCode = DockerImageCode.FromImageAsset("src/PersonalInfrastructure.GamingPC.ScheduledShutdown.Lambda/src/PersonalInfrastructure.GamingPC.ScheduledShutdown.Lambda");
            DockerImageFunction gamingPcShutdownLambda = new DockerImageFunction(this, "gamingPCShutdownLambda",
                new DockerImageFunctionProps()
                {
                    Architecture = Architecture.X86_64,
                    Code = gamingPcShutdownLambdaDockerImageCode,
                    Description = "Function to turn off gaming PC",
                    Timeout = Duration.Seconds(60) 
                }
            );
            
            // schedule
            Rule rule = new Rule(this, "Rule", new RuleProps()
            {
                Schedule = Schedule.Cron(new CronOptions()
                {
                    Minute = "0",
                    Hour = "8"
                }) 
            });
            rule.AddTarget(new LambdaFunction(gamingPcShutdownLambda));
        }
    }
}
