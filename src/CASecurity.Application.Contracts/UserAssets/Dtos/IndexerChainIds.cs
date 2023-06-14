using System.Collections.Generic;

namespace CASecurity.UserAssets.Dtos;

public class IndexerChainIds
{
    public List<UserChainInfo> CaHolderManagerInfo { get; set; }
}

public class UserChainInfo
{
    public string ChainId { get; set; }
}