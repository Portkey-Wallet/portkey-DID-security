using CAVerifierServer.Grains.State;
using Orleans;
using Orleans.Providers;

namespace CAVerifierServer.Grains.Grain;

[StorageProvider(ProviderName = "Default")]
public class SecurityGrain : Grain<SecurityState>,
    ISecurityGrain
{
    public override async Task OnActivateAsync()
    {
        await ReadStateAsync();
        await base.OnActivateAsync();
    }

    public override async Task OnDeactivateAsync()
    {
        await WriteStateAsync();
        await base.OnDeactivateAsync();
    }


    public async Task<bool> IsUserIpInWhiteListAsync(string ip)
    {
        var dic = State.IpWhiteListDic;
        if (dic == null || dic.Count == 0)
        {
            return false;
        }

        var expireTime = dic[ip];
        if (DateTime.Now <= expireTime)
        {
            return true;
        }

        dic.Remove(ip);
        await WriteStateAsync();
        return false;
    }

    public async Task AddUserIpToWhiteListAsync(string userIp)
    {
        State.IpWhiteListDic.Add(userIp,
            DateTime.Now.Add(TimeSpan.FromDays(15)));
        await WriteStateAsync();
    }
}