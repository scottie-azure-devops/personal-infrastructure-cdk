using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.SSM;
using Constructs;

namespace PersonalInfrastructure;

public class GamingPcStack : Stack
{
    internal GamingPcStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        // SSM parameters
        IStringParameter gamingPcSecurityGroupIpv4Cidr = StringParameter.FromStringParameterName(this, "IPV4CIDR", "/personal-infrastructure-cdk/ipv4-cidr");
        IStringParameter gamingPcSecurityGroupIpv6Cidr = StringParameter.FromStringParameterName(this, "IPV6CIDR", "/personal-infrastructure-cdk/ipv6-cidr");

        // networking
        Vpc gamingPcVpc = new Vpc(this, "GamingPCVPC", new VpcProps {
            IpAddresses = IpAddresses.Cidr("10.0.0.0/16"),
            NatGateways = 0
        });
        ISelectedSubnets subnetSelection = gamingPcVpc.SelectSubnets(new SubnetSelection {
            SubnetType = SubnetType.PUBLIC
        });
        SecurityGroup gamingPcSecurityGroup = new SecurityGroup(this, "GamingPCSecurityGroup", new SecurityGroupProps()
        {
            AllowAllOutbound = true,
            AllowAllIpv6Outbound = true,
            Vpc = gamingPcVpc
        });
        // allow inbound RDP traffic
        gamingPcSecurityGroup.AddIngressRule(Peer.Ipv4(gamingPcSecurityGroupIpv4Cidr.StringValue), Port.Tcp(3389));
        gamingPcSecurityGroup.AddIngressRule(Peer.Ipv6(gamingPcSecurityGroupIpv6Cidr.StringValue), Port.Tcp(3389));
        
        // IAM role
        Role gamingPcInstanceRole = new Role(this, "GamingPCInstanceRole", new RoleProps {
            AssumedBy = new ServicePrincipal("ec2.amazonaws.com"),
            ManagedPolicies = new IManagedPolicy[]
            {
                // required for installing drivers: https://docs.aws.amazon.com/AWSEC2/latest/WindowsGuide/install-nvidia-driver.html
                ManagedPolicy.FromAwsManagedPolicyName("AmazonS3ReadOnlyAccess") 
            }
        });

        // EC2 instance
        Instance_ gamingPc = new Instance_(this, "GamingPCInstance", new InstanceProps()
        {
            BlockDevices = new IBlockDevice[]
            {
                new BlockDevice()
                {
                    DeviceName = "/dev/sda1",
                    Volume = new BlockDeviceVolume(new EbsDeviceProps()
                    {
                        DeleteOnTermination = true,
                        VolumeSize = 100,
                        VolumeType = EbsDeviceVolumeType.GP3
                    })
                }
            },
            MachineImage = new WindowsImage(WindowsVersion.WINDOWS_SERVER_2022_ENGLISH_FULL_BASE),
            InstanceName = "GamingPC",
            InstanceType = new InstanceType("g5.2xlarge"),
            KeyName = "gaming-pc",
            Role = gamingPcInstanceRole,
            SecurityGroup = gamingPcSecurityGroup,
            VpcSubnets = new SubnetSelection()
            {
                SubnetType = SubnetType.PUBLIC
            },
            Vpc = gamingPcVpc 
        });
    }
}