using AElf.Indexing.Elasticsearch.Options;
using AElf.OpenTelemetry;
using CASecurity.EntityEventHandler.Core;
using CASecurity.MongoDB;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.EventBus.RabbitMq;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.Tokens;

namespace CASecurity;

[DependsOn(typeof(AbpAutofacModule),
    typeof(CASecurityMongoDbModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(CASecurityEntityEventHandlerCoreModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(OpenTelemetryModule),
    typeof(AbpEventBusRabbitMqModule))]
public class CASecurityEntityEventHandlerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        ConfigureTokenCleanupService();
        //ConfigureEsIndexCreation();
        context.Services.AddHostedService<CASecurityHostedService>();
    }
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
    }

    //Create the ElasticSearch Index based on Domain Entity
    private void ConfigureEsIndexCreation()
    {
        Configure<IndexCreateOption>(x => { x.AddModule(typeof(CASecurityDomainModule)); });
    }
    
    // TODO Temporary Needed fixed later.
    private void ConfigureTokenCleanupService()
    {
        Configure<TokenCleanupOptions>(x => x.IsCleanupEnabled = false);
    }
}