using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Events;
using Amazon.CDK.AWS.Events.Targets;
using Amazon.CDK.AWS.IAM;
using Constructs;

namespace PersonalInfrastructure
{
    public class Ec2ShutdownLambdaStack : Stack
    {
        internal Ec2ShutdownLambdaStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            // IAM
            Role gamingPcShutdownLambdaFunctionExecutionRole = new Role(this, "DockerFunctionExecutionRole", new RoleProps {
                AssumedBy = new ServicePrincipal("lambda.amazonaws.com"),
                ManagedPolicies = new IManagedPolicy[]
                {
                    new ManagedPolicy(this, "ManagedPolicy", new ManagedPolicyProps
                    {
                        Document = new PolicyDocument(new PolicyDocumentProps
                        {
                            Statements = new []
                            {
                                new PolicyStatement(new PolicyStatementProps
                                {
                                    Actions = new []
                                    {
                                        "ec2:Describe*",
                                        "ec2:StopInstances"
                                    },
                                    Effect = Effect.ALLOW,
                                    Resources = new [] { "*" }
                                }),
                                new PolicyStatement(new PolicyStatementProps
                                {
                                    Actions = new []
                                    {
                                        "logs:CreateLogGroup",
                                        "logs:CreateLogStream",
                                        "logs:PutLogEvents"
                                    },
                                    Effect = Effect.ALLOW,
                                    Resources = new [] { "*" }
                                })
                            }
                        })
                    })
                }
            });
            
            // Lambda function
            DockerImageCode gamingPcShutdownLambdaDockerImageCode = DockerImageCode.FromImageAsset("src/PersonalInfrastructure.GamingPC.ScheduledShutdown.Lambda/src/PersonalInfrastructure.GamingPC.ScheduledShutdown.Lambda");
            DockerImageFunction gamingPcShutdownLambda = new DockerImageFunction(this, "gamingPCShutdownLambda",
                new DockerImageFunctionProps()
                {
                    Architecture = Architecture.X86_64,
                    Code = gamingPcShutdownLambdaDockerImageCode,
                    Description = "Function to turn off gaming PC",
                    Role = gamingPcShutdownLambdaFunctionExecutionRole,
                    Timeout = Duration.Seconds(600) 
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
