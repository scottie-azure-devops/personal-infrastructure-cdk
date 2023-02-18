using Amazon.CDK;
using Amazon.CDK.AWS.Cloud9;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.SSM;
using Constructs;

namespace PersonalInfrastructure;

public class Cloud9Stack : Stack
{
    internal Cloud9Stack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        // SSM parameters
        IStringParameter cloud9OwnerArn = StringParameter.FromStringParameterName(this, "Cloud9OwnerARN", "/personal-infrastructure-cdk/cloud9-owner-arn");
        
        // networking
        Vpc cloud9Vpc = new Vpc(this, "Cloud9VPC", new VpcProps {
            IpAddresses = IpAddresses.Cidr("10.1.0.0/16"),
            NatGateways = 0
        });
        
        // Cloud 9 environment
        CfnEnvironmentEC2 cloud9Environment = new CfnEnvironmentEC2(this, "Cloud9", new CfnEnvironmentEC2Props()
        {
            AutomaticStopTimeMinutes = 30,
            ConnectionType = "CONNECT_SSH",
            ImageId = "amazonlinux-2-x86_64",
            InstanceType = "m5.large",
            Name = "scottie-cloud9",
            OwnerArn = cloud9OwnerArn.StringValue,
            // launching instance into public subnet for SSH
            SubnetId = cloud9Vpc.PublicSubnets[0].SubnetId
        });
    }
}