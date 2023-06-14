using System;
using AElf.Indexing.Elasticsearch;
using Nest;

namespace CASecurity.Entities.Es;

public class CAHolderIndex : CASecurityEsEntity<Guid>, IIndexBuild
{
    [Keyword] public Guid UserId { get; set; }
    [Keyword] public string CaHash { get; set; }
    [Keyword] public string NickName { get; set; }
    public DateTime CreateTime { get; set; }
}