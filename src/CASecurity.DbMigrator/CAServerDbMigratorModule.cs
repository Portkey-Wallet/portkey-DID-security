using CASecurity.MongoDB;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace CASecurity.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(CASecurityMongoDbModule),
    typeof(CASecurityApplicationContractsModule)
    )]
public class CASecurityDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
    }
}
