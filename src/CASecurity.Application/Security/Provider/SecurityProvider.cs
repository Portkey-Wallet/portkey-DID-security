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
    
    Task<CAHolderIndex> GetCaHolderIndexAsync(Guid userId);
    
    // Task<IndexerTokenInfos> GetUserTokenInfoAsync(List<CAAddressInfo> caAddressInfos, string symbol, int inputSkipCount,
    //     int inputMaxResultCount);
}

public class SecurityProvider: ISecurityProvider, ISingletonDependency
{
    private readonly INESTRepository<ContactIndex, Guid> _contactIndexRepository;
    private readonly INESTRepository<CAHolderIndex, Guid> _caHolderIndexRepository;

    public SecurityProvider(INESTRepository<ContactIndex, Guid> contactIndexRepository,
        INESTRepository<CAHolderIndex, Guid> caHolderIndexRepository)
    {
        _contactIndexRepository = contactIndexRepository;
        _caHolderIndexRepository = caHolderIndexRepository;
    }
    
    public async Task<CAHolderIndex> GetCaHolderIndexAsync(Guid paramUserId)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<CAHolderIndex>, QueryContainer>>() { };
        mustQuery.Add(q => q.Term(i => i.Field(f => f.UserId).Value(paramUserId)));

        QueryContainer Filter(QueryContainerDescriptor<CAHolderIndex> f) => f.Bool(b => b.Must(mustQuery));
        var caHolder = await _caHolderIndexRepository.GetAsync(Filter);
        return caHolder;
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