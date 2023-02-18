using System.Collections.Generic;
using Amazon.CDK;

namespace PersonalInfrastructure
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new Cloud9Stack(app, "Cloud9Stack", new StackProps()
            {
                Tags = new Dictionary<string, string>()
                {
                    {
                        "Environment", "Production"
                    }
                }
            });
            new Ec2ShutdownLambdaStack(app, "EC2ShutdownLambdaStack", new StackProps()
            {
                Tags = new Dictionary<string, string>()
                {
                    {
                        "Environment", "Production"
                    }
                }
            });
            new GamingPcStack(app, "GamingPCStack", new StackProps()
            {
                Tags = new Dictionary<string, string>()
                {
                    {
                        "Environment", "Production"
                    },
                    {
                        "NightlyShutdown", "True"
                    }
                }
            });
            app.Synth();
        }
    }
}
