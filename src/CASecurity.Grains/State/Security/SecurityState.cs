using System.Data;

namespace CASecurity.Grains.State;

public class SecurityState
{
    public Dictionary<string, DateTime> IpWhiteListDic { get; set; } = new();


}