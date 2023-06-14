using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace CASecurity.EntityEventHandler.Core
{
    [DependsOn(typeof(AbpAutoMapperModule), typeof(CASecurityApplicationModule),
        typeof(CASecurityApplicationContractsModule))]
    public class CASecurityEntityEventHandlerCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                //Add all mappings defined in the assembly of the MyModule class
                options.AddMaps<CASecurityEntityEventHandlerCoreModule>();
            });
        }
    }
}