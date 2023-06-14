using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CAServer.Entities.Es;
using CAServer.UserAssets.Dtos;

namespace CAServer.UserAssets.Provider;

public interface IUserAssetsProvider
{
    Task<IndexerTokenInfos> GetUserTokenInfoAsync(List<CAAddressInfo> caAddressInfos, string symbol, int inputSkipCount,
        int inputMaxResultCount);

    Task<IndexerChainIds> GetUserChainIdsAsync(List<string> userCaAddresses);

    Task<List<UserTokenIndex>> GetUserDefaultTokenSymbolAsync(Guid userId);
    Task<List<UserTokenIndex>> GetUserIsDisplayTokenSymbolAsync(Guid userId);
    Task<List<(string, string)>> GetUserNotDisplayTokenAsync(Guid userId);
}