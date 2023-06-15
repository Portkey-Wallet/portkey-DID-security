using System;
using AElf.Indexing.Elasticsearch;
using Nest;

namespace CASecurity.Entities.Es;

public class GuardianIndex : CASecurityEsEntity<string>, IIndexBuild
{
    [Keyword] public string Identifier { get; set; }
    [Keyword] public string IdentifierHash { get; set; }
    [Keyword] public string Salt { get; set; }
    public DateTime CreateTime { get; set; } = DateTime.UtcNow;
}