using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AElf.Types;
using CASecurity.Common;
using CASecurity.Options;
using CASecurity.UserAssets;
using CASecurity.Account;
using CASecurity.Grains.Grain;
using CASecurity.IpWhiteList;
using CASecurity.Security.Provider;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Orleans;
using Volo.Abp.DependencyInjection;

namespace CASecurity.Security;

public class SecurityAppService : ISecurityAppService, ISingletonDependency
{
    private readonly IClusterClient _clusterClient;
    private readonly IContractProvider _contractProvider;
    private readonly ChainOptions _chainOptions;
    private readonly IUserAssetsAppService _userAssetsAppService;
    private readonly ISecurityProvider _securityProvider;

    public SecurityAppService(
        IClusterClient clusterClient,
        IContractProvider contractProvider,
        IOptions<ChainOptions> chainOptions,
        IUserAssetsAppService userAssetsAppService,
        ISecurityProvider securityProvider)
    {
        _clusterClient = clusterClient;
        _contractProvider = contractProvider;
        _userAssetsAppService = userAssetsAppService;
        _securityProvider = securityProvider;
        _chainOptions = chainOptions.Value;
    }

    public async Task<ResponseResultDto<IpCheckResultDtos>> IsUserIpInWhiteListAsync(CheckUserIpRequestDto requestDto)
    {
        var grain = _clusterClient.GetGrain<ISecurityGrain>(requestDto.Ip);
        var result = await grain.IsUserIpInWhiteListAsync(requestDto.Ip);
        return new ResponseResultDto<IpCheckResultDtos>()
        {
            Success = true,
            Data = new IpCheckResultDtos()
            {
                IsInWhiteList = result
            }
        };
    }

    public async Task AddIpToWhiteListAsync(AddUserIpToWhiteListRequestDto request)
    {
        var grain = _clusterClient.GetGrain<ISecurityGrain>(request.UserIp);

        var result = await grain.IsUserIpInWhiteListAsync(request.UserIp);
        if (result)
        {
            return;
        }

        var caHolderIndex = await _securityProvider.GetCaHolderIndexAsync(request.UserId);
        var caHash = caHolderIndex.CaHash;
        var caAddress = new List<string>();
        var caAddressInfos = new List<CAAddressInfo>();
        foreach (var chainInfo in _chainOptions.ChainInfos)
        {
            caAddress.Add(chainInfo.Value.ContractAddress);
            caAddressInfos.Add(new CAAddressInfo
            {
                ChainId = chainInfo.Key,
                CaAddress = chainInfo.Value.ContractAddress
            });

            var output =
                await _contractProvider.GetHolderInfoAsync(Hash.LoadFromHex(caHash), null, chainInfo.Value.ChainId);
            var count = output.GuardianList.Guardians.Count;
            if (count >= 2)
            {
                await grain.AddUserIpToWhiteListAsync(request.UserIp);
                return;
            }

            var managerInfos = output.ManagerInfos;

            var devices = managerInfos?.Select(t => GetDevices(t.ExtraData)).Where(t => !t.IsNullOrEmpty()).Distinct()
                .ToList();
            if (devices is { Count: >= 2 })
            {
                await grain.AddUserIpToWhiteListAsync(request.UserIp);
                return;
            }
        }

        var contactNum = await _securityProvider.GetContactCountAsync(request.UserId);
        if (contactNum >= 1)
        {
            await grain.AddUserIpToWhiteListAsync(request.UserIp);
        }

        var requestDto = new GetTokenRequestDto()
        {
            CaAddresses = caAddress,
            CaAddressInfos = caAddressInfos
        };
        var tokenAsync = await _userAssetsAppService.GetTokenAsync(requestDto);
        if (tokenAsync.Data != null)
        {
            if (tokenAsync.Data.Any(token => int.Parse(token.Balance) > 0))
            {
                await grain.AddUserIpToWhiteListAsync(request.UserIp);
            }
        }
    }

    private string GetDevices(string jsonStr)
    {
        if (jsonStr.IsNullOrWhiteSpace())
        {
            return string.Empty;
        }

        var jo = JObject.Parse(jsonStr);
        return jo["deviceInfo"]?.ToString();
    }
}