using System.Collections.Generic;
using Amazon.CDK;

namespace PersonalInfrastructure
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new Ec2ShutdownLambdaStack(app, "EC2ShutdownLambdaStack");
            new GamingPcStack(app, "GamingPCStack", new StackProps()
            {
                Tags = new Dictionary<string, string>()
                {
                    {
                        "NightlyShutdown", "True"
                    }
                }
            });
            app.Synth();
        }
    }
}
