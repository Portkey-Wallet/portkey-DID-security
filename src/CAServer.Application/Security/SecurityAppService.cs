using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AElf.Types;
using CAServer.CAActivity.Provider;
using CAServer.Common;
using CAServer.Device;
using CAServer.Options;
using CAServer.security;
using CAServer.UserAssets;
using CAVerifierServer.Account;
using CAVerifierServer.Grains.Grain;
using CAVerifierServer.IpWhiteList;
using CAVerifierServer.Security.Provider;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Orleans;
using Volo.Abp.DependencyInjection;

namespace CAVerifierServer.Security;

public class SecurityAppService : ISecurityAppService, ISingletonDependency
{
    private readonly IClusterClient _clusterClient;
    private readonly IActivityProvider _activityProvider;
    private readonly IContractProvider _contractProvider;
    private ChainOptions _chainOptions;
    private readonly IUserAssetsAppService _userAssetsAppService;
    private readonly ISecurityProvider _securityProvider;

    public SecurityAppService(IActivityProvider activityProvider,
        IClusterClient clusterClient,
        IContractProvider contractProvider,
        IOptions<ChainOptions> chainOptions,
        IUserAssetsAppService userAssetsAppService,
        ISecurityProvider securityProvider)
    {
        _activityProvider = activityProvider;
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
        var caHolderIndex = await _activityProvider.GetCaHolderIndexAsync(request.UserId);
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
                break;
            }

            var managerInfos = output.ManagerInfos;
            foreach (var manager in managerInfos)
            {
                //TODO How to transfer to DevicesInfo;
                var data = manager.ExtraData;

                //await _deviceAppService.EncryptExtraDataAsync(data, caHash);
            }

            var device = managerInfos.Select(t => t.ExtraData).Distinct();
        }
        
        var contactNum = await _securityProvider.GetContactCountAsync(request.UserId);
        if (contactNum > 2)
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

    public Task RemoveIpFromWhiteListAsync(string userIp)
    {
        throw new NotImplementedException();
    }
}