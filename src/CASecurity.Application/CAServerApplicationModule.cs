using System.IdentityModel.Tokens.Jwt;
using CASecurity.Common;
using CASecurity.Grains;
using CASecurity.Options;
using CASecurity.Signature;
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

namespace CASecurity;

[DependsOn(
    typeof(CASecurityDomainModule),
    typeof(AbpAccountApplicationModule),
    typeof(CASecurityApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(CASecurityGrainsModule),
    typeof(CASecuritySignatureModule)
)]
public class CASecurityApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<CASecurityApplicationModule>(); });
        var configuration = context.Services.GetConfiguration();
        Configure<ChainOptions>(configuration.GetSection("Chains"));
        Configure<ContractOptions>(configuration.GetSection("ContractOptions"));
        context.Services.AddHttpClient();
        context.Services.AddScoped<JwtSecurityTokenHandler>();
        context.Services.AddScoped<IHttpClientService, HttpClientService>();
        context.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }
}