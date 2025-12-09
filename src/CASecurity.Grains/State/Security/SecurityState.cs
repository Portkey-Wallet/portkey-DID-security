namespace CASecurity.Grains.State;

[GenerateSerializer]
public class SecurityState
{
    [Id(0)] public DateTime? ExpireTime { get; set; }
}