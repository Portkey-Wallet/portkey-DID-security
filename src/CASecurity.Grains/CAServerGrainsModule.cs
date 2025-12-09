using CASecurity.Signature;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace CASecurity.Grains;

[DependsOn(typeof(CASecurityApplicationContractsModule),
    typeof(AbpAutoMapperModule),
    typeof(CASecuritySignatureModule))]
public class CASecurityGrainsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<CASecurityGrainsModule>(); });

        var configuration = context.Services.GetConfiguration();
        var connStr = configuration["GraphQL:Configuration"];
    }
}