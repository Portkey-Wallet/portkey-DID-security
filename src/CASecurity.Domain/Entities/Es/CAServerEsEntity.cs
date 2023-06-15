using Volo.Abp.Domain.Entities;

namespace CASecurity.Entities.Es;

public abstract class CASecurityEsEntity<TKey> : Entity, IEntity<TKey>
{
    public virtual TKey Id { get; set; }

    public override object[] GetKeys()
    {
        return new object[] { Id };
    }
}