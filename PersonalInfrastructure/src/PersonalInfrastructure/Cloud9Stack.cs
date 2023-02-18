using Amazon.CDK;
using Amazon.CDK.AWS.Cloud9;
using Amazon.CDK.AWS.EC2;
using Constructs;

namespace PersonalInfrastructure;

public class Cloud9Stack : Stack
{
    internal Cloud9Stack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        // networking
        Vpc cloud9Vpc = new Vpc(this, "Cloud9VPC", new VpcProps {
            IpAddresses = IpAddresses.Cidr("10.1.0.0/16"),
            NatGateways = 0
        });
        
        // Cloud 9 environment
        CfnEnvironmentEC2 cloud9Environment = new CfnEnvironmentEC2(this, "Cloud9", new CfnEnvironmentEC2Props()
        {
            AutomaticStopTimeMinutes = 30,
            InstanceType = "m5.large",
            Name = "scottie-cloud9",
            // launching instance into public subnet for SSH
            SubnetId = cloud9Vpc.PublicSubnets[0].SubnetId
        });
    }
}