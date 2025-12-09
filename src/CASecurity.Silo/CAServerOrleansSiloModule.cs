using AElf.OpenTelemetry;
using CASecurity.Common;
using CASecurity.Grains;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace CASecurity.Silo;

[DependsOn(typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(CASecurityGrainsModule),
    typeof(OpenTelemetryModule)
)]
public class CASecurityOrleansSiloModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHostedService<CASecurityHostedService>();
        var configuration = context.Services.GetConfiguration();
        //ConfigureEsIndexCreation();
        // Configure<GrainOptions>(configuration.GetSection("Contract"));
        // Configure<ChainOptions>(configuration.GetSection("Chains"));
    }
    
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        ConfigurationProvidersHelper.DisplayConfigurationProviders(context);
    }
}