using System;
using Volo.Abp.Domain.Entities;

namespace CASecurity.Entities;

[Serializable]
public abstract class CASecurityEntity <TKey> : Entity, IEntity<TKey>
{
    /// <inheritdoc/>
    public virtual TKey Id { get; set; }

    protected CASecurityEntity()
    {

    }

    protected CASecurityEntity(TKey id)
    {
        Id = id;
    }

    public override object[] GetKeys()
    {
        return new object[] {Id};
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"[ENTITY: {GetType().Name}] Id = {Id}";
    }
}