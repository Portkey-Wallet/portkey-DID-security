﻿using System.IdentityModel.Tokens.Jwt;
using CAServer.AccountValidator;
using CAServer.Common;
using CAServer.Grains;
using CAServer.IpInfo;
using CAServer.Options;
using CAServer.Search;
using CAServer.Settings;
using CAServer.Signature;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace CAServer;

[DependsOn(
    typeof(CAServerDomainModule),
    typeof(AbpAccountApplicationModule),
    typeof(CAServerApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(CAServerGrainsModule),
    typeof(CAServerSignatureModule)
)]
public class CAServerApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<CAServerApplicationModule>(); });
        var configuration = context.Services.GetConfiguration();
        context.Services.AddSingleton<ISearchService, UserTokenSearchService>();
        context.Services.AddSingleton<ISearchService, ContactSearchService>();
        context.Services.AddSingleton<ISearchService, ChainsInfoSearchService>();
        context.Services.AddSingleton<ISearchService, AccountRecoverySearchService>();
        context.Services.AddSingleton<ISearchService, AccountRegisterSearchService>();
        context.Services.AddSingleton<ISearchService, CAHolderSearchService>();
        context.Services.AddSingleton<ISearchService, OrderSearchService>();
        context.Services.AddSingleton<ISearchService, UserExtraInfoSearchService>();
        context.Services.AddSingleton<ISearchService, NotifySearchService>();
        context.Services.AddSingleton<ISearchService, GuardianSearchService>();
        
        
        Configure<ChainOptions>(configuration.GetSection("Chains"));
        Configure<DeviceOptions>(configuration.GetSection("EncryptionInfo"));
        context.Services.AddSingleton<IAccountValidator, EmailValidator>();
        context.Services.AddSingleton<IAccountValidator, PhoneValidator>();
        Configure<ContractOptions>(configuration.GetSection("ContractOptions"));
        context.Services.AddHttpClient();
        context.Services.AddScoped<JwtSecurityTokenHandler>();
        context.Services.AddScoped<IHttpClientService, HttpClientService>();
        context.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }
}