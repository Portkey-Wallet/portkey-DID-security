using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CASecurity.CAActivity.Provider;
using CASecurity.Common;
using CASecurity.Entities.Es;
using CASecurity.Options;
using CASecurity.Tokens;
using CASecurity.UserAssets.Dtos;
using CASecurity.UserAssets.Provider;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans.Runtime;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Users;
using Token = CASecurity.UserAssets.Dtos.Token;
using TokenInfo = CASecurity.UserAssets.Provider.TokenInfo;

namespace CASecurity.UserAssets;

[RemoteService(false)]
[DisableAuditing]
public class UserAssetsAppService : CASecurityAppService, IUserAssetsAppService
{
    private readonly IUserAssetsProvider _userAssetsProvider;

    public UserAssetsAppService(
        IUserAssetsProvider userAssetsProvider)
    {
        _userAssetsProvider = userAssetsProvider;
    }

    public async Task<GetTokenDto> GetTokenAsync(GetTokenRequestDto requestDto)
    {
        try
        {
            var caAddressInfos = requestDto.CaAddressInfos;
            if (caAddressInfos == null)
            {
                caAddressInfos = requestDto.CaAddresses.Select(address => new CAAddressInfo { CaAddress = address })
                    .ToList();
            }

            var res = await _userAssetsProvider.GetUserTokenInfoAsync(caAddressInfos, "",
                0, requestDto.SkipCount + requestDto.MaxResultCount);

            res.CaHolderTokenBalanceInfo.Data =
                res.CaHolderTokenBalanceInfo.Data.Where(t => t.TokenInfo != null).ToList();

            var chainInfos = await _userAssetsProvider.GetUserChainIdsAsync(requestDto.CaAddresses);
            var chainIds = chainInfos.CaHolderManagerInfo.Select(c => c.ChainId).Distinct().ToList();

            var dto = new GetTokenDto
            {
                Data = new List<Token>(),
                TotalRecordCount = 0
            };

            var userDefaultTokenSymbols = await _userAssetsProvider.GetUserDefaultTokenSymbolAsync(requestDto.UserId);

            var userTokenSymbols = new List<UserTokenIndex>();

            userTokenSymbols.AddRange(userDefaultTokenSymbols);
            userTokenSymbols.AddRange(await _userAssetsProvider.GetUserIsDisplayTokenSymbolAsync(requestDto.UserId));

            if (userTokenSymbols.IsNullOrEmpty())
            {
                Logger.LogError("get no result from current user {id}", requestDto.UserId);
                return dto;
            }

            var list = new List<Token>();

            foreach (var symbol in userTokenSymbols)
            {
                if (!chainIds.Contains(symbol.Token.ChainId))
                {
                    continue;
                }

                var tokenInfo = res.CaHolderTokenBalanceInfo.Data.FirstOrDefault(t =>
                    t.TokenInfo.Symbol == symbol.Token.Symbol && t.ChainId == symbol.Token.ChainId);
                if (tokenInfo == null)
                {
                    var data = await _userAssetsProvider.GetUserTokenInfoAsync(caAddressInfos,
                        symbol.Token.Symbol, 0, requestDto.CaAddresses.Count);
                    tokenInfo = data.CaHolderTokenBalanceInfo.Data.FirstOrDefault(
                        t => t.ChainId == symbol.Token.ChainId);
                    tokenInfo ??= new IndexerTokenInfo
                    {
                        Balance = 0,
                        ChainId = symbol.Token.ChainId,
                        TokenInfo = new TokenInfo
                        {
                            Decimals = symbol.Token.Decimals,
                            Symbol = symbol.Token.Symbol,
                            TokenContractAddress = symbol.Token.Address
                        }
                    };
                }
                else
                {
                    res.CaHolderTokenBalanceInfo.Data.Remove(tokenInfo);
                }

                var token = ObjectMapper.Map<IndexerTokenInfo, Token>(tokenInfo);

                list.Add(token);
            }

            if (!res.CaHolderTokenBalanceInfo.Data.IsNullOrEmpty())
            {
                var userNotDisplayTokenAsync =
                    await _userAssetsProvider.GetUserNotDisplayTokenAsync(requestDto.UserId);

                while (list.Count < requestDto.MaxResultCount + requestDto.SkipCount)
                {
                    var userAsset = res.CaHolderTokenBalanceInfo.Data.FirstOrDefault();
                    if (userAsset == null)
                    {
                        break;
                    }

                    if (!userNotDisplayTokenAsync.Contains((userAsset.TokenInfo.Symbol, userAsset.ChainId)))
                    {
                        list.Add(ObjectMapper.Map<IndexerTokenInfo, Token>(userAsset));
                    }

                    res.CaHolderTokenBalanceInfo.Data.Remove(userAsset);
                }
            }

            dto.TotalRecordCount = list.Count;

            var resultList = new List<Token>();

            list.Sort((t1, t2) => t1.Symbol != t2.Symbol
                ? string.Compare(t1.Symbol, t2.Symbol, StringComparison.Ordinal)
                : string.Compare(t1.ChainId, t2.ChainId, StringComparison.Ordinal));

            resultList.AddRange(list.Where(t => userDefaultTokenSymbols.Select(s => s.Token.Symbol).Contains(t.Symbol))
                .ToList());
            resultList.AddRange(list.Where(t => !userDefaultTokenSymbols.Select(s => s.Token.Symbol).Contains(t.Symbol))
                .ToList());

            resultList = resultList.Skip(requestDto.SkipCount).Take(requestDto.MaxResultCount).ToList();

            dto.Data.AddRange(resultList);

            return dto;
        }
        catch (Exception e)
        {
            Logger.LogError(e, "GetTokenAsync Error. {dto}", requestDto);
            return new GetTokenDto { Data = new List<Token>(), TotalRecordCount = 0 };
        }
    }
}