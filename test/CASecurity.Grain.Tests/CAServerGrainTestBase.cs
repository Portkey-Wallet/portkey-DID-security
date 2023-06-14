using Orleans.TestingHost;
using Volo.Abp.Caching;

namespace CASecurity.Grain.Tests;

public class CASecurityGrainTestBase :CASecurityTestBase<CASecurityGrainTestModule>
{
    protected readonly TestCluster Cluster;

    public CASecurityGrainTestBase()
    {
        Cluster = GetRequiredService<ClusterFixture>().Cluster;

    }
}