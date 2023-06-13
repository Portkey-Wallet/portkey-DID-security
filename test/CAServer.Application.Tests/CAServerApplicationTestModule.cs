using System;
using System.Collections.Generic;
using System.Configuration;
using CAServer.Grain.Tests;
using CAServer.IpInfo;
using CAServer.Options;
using CAServer.Search;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;

namespace CAServer;

[DependsOn(
    typeof(CAServerApplicationModule),
    typeof(AbpEventBusModule),
    typeof(CAServerGrainTestModule),
    typeof(CAServerDomainTestModule)
)]
public class CAServerApplicationTestModule : AbpModule
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

        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<CAServerApplicationModule>(); });

        
        context.Services.Configure<CAServer.Options.ChainOptions>(option =>
        {
            option.ChainInfos = new Dictionary<string, CAServer.Options.ChainInfo>
            {
                {
                    "TEST", new CAServer.Options.ChainInfo()
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