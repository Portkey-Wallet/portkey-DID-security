using System;
using System.Collections.Generic;
using CASecurity.Grain.Tests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;

namespace CASecurity;

[DependsOn(
    typeof(CASecurityApplicationModule),
    typeof(AbpEventBusModule),
    typeof(CASecurityGrainTestModule),
    typeof(CASecurityDomainTestModule)
)]
public class CASecurityApplicationTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // load config from [appsettings.Development.json]
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
            .AddEnvironmentVariables();

        var configuration = builder.Build();

        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<CASecurityApplicationModule>(); });

        
        context.Services.Configure<CASecurity.Options.ChainOptions>(option =>
        {
            option.ChainInfos = new Dictionary<string, CASecurity.Options.ChainInfo>
            {
                {
                    "TEST", new CASecurity.Options.ChainInfo()
                    {
                        BaseUrl = "http://127.0.0.1:6889",
                        ChainId = "TEST",
                        PrivateKey = "28d2520e2c480ef6f42c2803dcf4348807491237fd294c0f0a3d7c8f9ab8fb91"
                    }
                }
            };
        });
        
        base.ConfigureServices(context);
    }
}