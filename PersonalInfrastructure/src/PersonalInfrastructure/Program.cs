using Amazon.CDK;

namespace PersonalInfrastructure
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new Ec2ShutdownLambdaStack(app, "Ec2ShutdownLambdaStack");
            app.Synth();
        }
    }
}
