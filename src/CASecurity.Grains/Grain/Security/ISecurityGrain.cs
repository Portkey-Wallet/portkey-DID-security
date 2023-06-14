using Orleans;

namespace CASecurity.Grains.Grain;

public interface ISecurityGrain : IGrainWithStringKey
{
    Task<bool> IsUserIpInWhiteListAsync(string ip);
    
    Task AddUserIpToWhiteListAsync(string userIp);
}