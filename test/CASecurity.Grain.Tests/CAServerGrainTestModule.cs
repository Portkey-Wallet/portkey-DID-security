﻿using CAServer.Grains;
using CAServer.Signature;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectMapping;

namespace CAServer.Grain.Tests;

[DependsOn(
    typeof(CAServerGrainsModule),
    typeof(CAServerDomainTestModule),
    typeof(CAServerDomainModule),
    typeof(AbpCachingModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpObjectMappingModule),
    typeof(CAServerSignatureModule)
)]
public class CAServerGrainTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<IClusterClient>(sp => sp.GetService<ClusterFixture>().Cluster.Client);
        context.Services.AddHttpClient();
    }
}