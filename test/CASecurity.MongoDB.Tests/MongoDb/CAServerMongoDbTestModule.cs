using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace CASecurity.MongoDB;

[DependsOn(
    typeof(CASecurityTestBaseModule),
    typeof(CASecurityMongoDbModule)
    )]
public class CASecurityMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // var stringArray = CASecurityMongoDbFixture.ConnectionString.Split('?');
        // var connectionString = stringArray[0].EnsureEndsWith('/') +
        //                            "Db_" +
        //                        Guid.NewGuid().ToString("N") + "/?" + stringArray[1];
        //
        // Configure<AbpDbConnectionOptions>(options =>
        // {
        //     options.ConnectionStrings.Default = connectionString;
        // });
    }
}
