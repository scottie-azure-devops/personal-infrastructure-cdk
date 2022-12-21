using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Constructs;

namespace PersonalInfrastructure
{
    public class PersonalInfrastructureStack : Stack
    {
        internal PersonalInfrastructureStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            DockerImageCode gamingPCShutdownLambdaDockerImageCode = DockerImageCode.FromImageAsset("src/PersonalInfrastructure.GamingPC.ScheduledShutdown.Lambda/src/PersonalInfrastructure.GamingPC.ScheduledShutdown.Lambda");
            DockerImageFunction gamingPCShutdownLambda = new DockerImageFunction(this, "gamingPCShutdownLambda",
                new DockerImageFunctionProps()
                {
                    Architecture = Architecture.ARM_64,
                    Code = gamingPCShutdownLambdaDockerImageCode,
                    Description = "Function to turn off gaming PC",
                    Timeout = Duration.Seconds(30) 
                }
            );
        }
    }
}
