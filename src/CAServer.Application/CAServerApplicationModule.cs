using System.IdentityModel.Tokens.Jwt;
using CAServer.AccountValidator;
using CAServer.Common;
using CAServer.Grains;
using CAServer.Options;
using CAServer.Search;
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
        Configure<ChainOptions>(configuration.GetSection("Chains"));
        context.Services.AddSingleton<IAccountValidator, EmailValidator>();
        context.Services.AddSingleton<IAccountValidator, PhoneValidator>();
        Configure<ContractOptions>(configuration.GetSection("ContractOptions"));
        context.Services.AddHttpClient();
        context.Services.AddScoped<JwtSecurityTokenHandler>();
        context.Services.AddScoped<IHttpClientService, HttpClientService>();
        context.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }
}