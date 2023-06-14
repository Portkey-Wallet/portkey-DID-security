using Orleans.TestingHost;
using Volo.Abp.Modularity;

namespace CASecurity.Orleans.TestBase;

public abstract class CASecurityOrleansTestBase<TStartupModule> : CASecurityTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected readonly TestCluster Cluster;

    public CASecurityOrleansTestBase()
    {
        Cluster = GetRequiredService<ClusterFixture>().Cluster;
    }
}