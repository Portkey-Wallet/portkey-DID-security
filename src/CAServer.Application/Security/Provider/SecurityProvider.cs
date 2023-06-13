using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AElf.Indexing.Elasticsearch;
using CAServer.Entities.Es;
using CAServer.UserAssets;
using CAServer.UserAssets.Provider;
using Nest;
using Volo.Abp.DependencyInjection;

namespace CAVerifierServer.Security.Provider;

public interface ISecurityProvider
{
    Task<long> GetContactCountAsync(Guid userId);
    
    // Task<IndexerTokenInfos> GetUserTokenInfoAsync(List<CAAddressInfo> caAddressInfos, string symbol, int inputSkipCount,
    //     int inputMaxResultCount);
}

public class SecurityProvider: ISecurityProvider, ISingletonDependency
{
    private readonly INESTRepository<ContactIndex, Guid> _contactIndexRepository;

    public SecurityProvider(INESTRepository<ContactIndex, Guid> contactIndexRepository)
    {
        _contactIndexRepository = contactIndexRepository;
    }
    
    public async Task<long> GetContactCountAsync(Guid userId)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<ContactIndex>, QueryContainer>>() { };
        mustQuery.Add(q => q.Term(i => i.Field(f => f.UserId).Value(userId)));
        mustQuery.Add(q => q.Term(i => i.Field(f => f.IsDeleted).Value(false)));

        QueryContainer Filter(QueryContainerDescriptor<ContactIndex> f) => f.Bool(b => b.Must(mustQuery));
        var contact = await _contactIndexRepository.GetListAsync(Filter);
        return contact.Item1;
    }
}