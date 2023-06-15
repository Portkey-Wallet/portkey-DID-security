namespace CASecurity.Entities;

public class MultiChainEntity<TKey> : CASecurityEntity<TKey>, IMultiChain
{
    public virtual int ChainId { get; set; }


    protected MultiChainEntity()
    {
    }

    protected MultiChainEntity(TKey id)
        : base(id)
    {
    }
}