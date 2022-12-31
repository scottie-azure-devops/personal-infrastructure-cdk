using System;
using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Constructs;

namespace PersonalInfrastructure;

public class GamingPcStack : Stack
{
    internal GamingPcStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        // networking
        Vpc gamingPcVpc = new Vpc(this, "GamingPCVPC", new VpcProps {
            IpAddresses = IpAddresses.Cidr("10.0.0.0/16")
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
        gamingPcSecurityGroup.AddIngressRule(Peer.Ipv4(System.Environment.GetEnvironmentVariable("GAMING_PC_IPV4_CIDR") ?? throw new InvalidOperationException()), Port.Tcp(3389));
        gamingPcSecurityGroup.AddIngressRule(Peer.Ipv6(System.Environment.GetEnvironmentVariable("GAMING_PC_IPV6_CIDR") ?? throw new InvalidOperationException()), Port.Tcp(3389));

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
                        VolumeSize = 50,
                        VolumeType = EbsDeviceVolumeType.GP3
                    })
                }
            },
            MachineImage = MachineImage.GenericWindows(new Dictionary<string, string>()
            {
                {
                    "us-east-1", "ami-09420ec0ca80e0435"
                }
            }),
            InstanceName = "GamingPC",
            InstanceType = new InstanceType("g4dn.xlarge"),
            KeyName = "gaming-pc",
            SecurityGroup = gamingPcSecurityGroup,
            Vpc = gamingPcVpc 
        });
    }
}