using CASecurity.Grains.State;
using Orleans;
using Orleans.Providers;

namespace CASecurity.Grains.Grain;

[StorageProvider(ProviderName = "Default")]
public class SecurityGrain : Grain<SecurityState>, ISecurityGrain
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

    public Task<bool> IsUserIpInWhiteListAsync(string ip)
    {
        return Task.FromResult(State.ExpireTime != null && DateTime.UtcNow <= State.ExpireTime);
    }

    public async Task AddUserIpToWhiteListAsync(string userIp)
    {
        State.ExpireTime = DateTime.UtcNow.Add(TimeSpan.FromDays(15));
        await WriteStateAsync();
    }
}