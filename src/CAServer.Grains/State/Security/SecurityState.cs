using System.Data;

namespace CAVerifierServer.Grains.State;

public class SecurityState
{
    public Dictionary<string, DateTime> IpWhiteListDic { get; set; }
    

}