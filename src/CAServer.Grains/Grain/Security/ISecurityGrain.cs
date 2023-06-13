using Orleans;

namespace CAVerifierServer.Grains.Grain;

public interface ISecurityGrain : IGrainWithStringKey
{
    Task<bool> IsUserIpInWhiteListAsync(string ip);
    
    Task AddUserIpToWhiteListAsync(string userIp);
}