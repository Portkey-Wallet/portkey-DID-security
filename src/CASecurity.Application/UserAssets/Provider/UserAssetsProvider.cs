using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AElf.Indexing.Elasticsearch;
using CAServer.Common;
using CAServer.Entities.Es;
using CAServer.UserAssets.Dtos;
using GraphQL;
using Nest;
using Volo.Abp.DependencyInjection;

namespace CAServer.UserAssets.Provider;

public class UserAssetsProvider : IUserAssetsProvider, ISingletonDependency
{
    private readonly IGraphQLHelper _graphQlHelper;
    private readonly INESTRepository<UserTokenIndex, Guid> _userTokenIndexRepository;

    public UserAssetsProvider(IGraphQLHelper graphQlHelper,
        INESTRepository<UserTokenIndex, Guid> userTokenIndexRepository)
    {
        _graphQlHelper = graphQlHelper;
        _userTokenIndexRepository = userTokenIndexRepository;
    }

    public async Task<IndexerChainIds> GetUserChainIdsAsync(List<string> userCaAddresses)
    {
        return await _graphQlHelper.QueryAsync<IndexerChainIds>(new GraphQLRequest
        {
            Query = @"
			    query($caAddresses:[String],$skipCount:Int!,$maxResultCount:Int!) {
                    caHolderManagerInfo(dto: {caAddresses:$caAddresses,skipCount:$skipCount,maxResultCount:$maxResultCount}){
                        chainId}
                }",
            Variables = new
            {
                caAddresses = userCaAddresses, skipCount = 0, maxResultCount = userCaAddresses.Count
            }
        });
    }

    public async Task<IndexerTokenInfos> GetUserTokenInfoAsync(List<CAAddressInfo> caAddressInfos, string symbol,
        int inputSkipCount, int inputMaxResultCount)
    {
        return await _graphQlHelper.QueryAsync<IndexerTokenInfos>(new GraphQLRequest
        {
            Query = @"
			    query($symbol:String,$caAddressInfos:[CAAddressInfo],$skipCount:Int!,$maxResultCount:Int!) {
                    caHolderTokenBalanceInfo(dto: {symbol:$symbol,caAddressInfos:$caAddressInfos,skipCount:$skipCount,maxResultCount:$maxResultCount}){
                        data{chainId,balance,caAddress,tokenIds,tokenInfo{symbol,tokenContractAddress,decimals,tokenName,totalSupply}},totalRecordCount}
                }",
            Variables = new
            {
                caAddressInfos = caAddressInfos, symbol, skipCount = inputSkipCount,
                maxResultCount = inputMaxResultCount
            }
        });
    }

    public async Task<List<UserTokenIndex>> GetUserDefaultTokenSymbolAsync(Guid userId)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<UserTokenIndex>, QueryContainer>>();
        mustQuery.Add(q => q.Term(i => i.Field(f => f.UserId).Value(userId)));
        mustQuery.Add(q => q.Term(i => i.Field(f => f.IsDefault).Value(true)));
        QueryContainer Filter(QueryContainerDescriptor<UserTokenIndex> f) => f.Bool(b => b.Must(mustQuery));

        IPromise<IList<ISort>> Sort(SortDescriptor<UserTokenIndex> s) => s.Descending(a => a.SortWeight)
            .Ascending(a => a.Token.Symbol).Ascending(a => a.Token.ChainId);

        var (totalCount, userTokenIndices) = await _userTokenIndexRepository.GetSortListAsync(Filter, sortFunc: Sort);

        if (totalCount <= 0 || userTokenIndices.IsNullOrEmpty())
        {
            return new List<UserTokenIndex>();
        }

        return userTokenIndices;
    }

    public async Task<List<UserTokenIndex>> GetUserIsDisplayTokenSymbolAsync(Guid userId)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<UserTokenIndex>, QueryContainer>>();
        mustQuery.Add(q => q.Term(i => i.Field(f => f.UserId).Value(userId)));
        mustQuery.Add(q => q.Term(i => i.Field(f => f.IsDefault).Value(false)));
        mustQuery.Add(q => q.Term(i => i.Field(f => f.IsDisplay).Value(true)));
        QueryContainer Filter(QueryContainerDescriptor<UserTokenIndex> f) => f.Bool(b => b.Must(mustQuery));

        IPromise<IList<ISort>> Sort(SortDescriptor<UserTokenIndex> s) => s.Descending(a => a.SortWeight)
            .Ascending(a => a.Token.Symbol).Ascending(a => a.Token.ChainId);

        var (totalCount, userTokenIndices) = await _userTokenIndexRepository.GetSortListAsync(Filter, sortFunc: Sort);

        if (totalCount <= 0 || userTokenIndices.IsNullOrEmpty())
        {
            return new List<UserTokenIndex>();
        }

        return userTokenIndices;
    }

    public async Task<List<(string, string)>> GetUserNotDisplayTokenAsync(Guid userId)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<UserTokenIndex>, QueryContainer>>();
        mustQuery.Add(q => q.Term(i => i.Field(f => f.UserId).Value(userId)));
        mustQuery.Add(q => q.Term(i => i.Field(f => f.IsDisplay).Value(false)));
        QueryContainer Filter(QueryContainerDescriptor<UserTokenIndex> f) => f.Bool(b => b.Must(mustQuery));

        var (totalCount, userTokenIndices) = await _userTokenIndexRepository.GetListAsync(Filter);

        if (totalCount <= 0 || userTokenIndices.IsNullOrEmpty())
        {
            return new List<(string, string)>();
        }

        return userTokenIndices.Select(t => (t.Token.Symbol, t.Token.ChainId)).ToList();
    }
}