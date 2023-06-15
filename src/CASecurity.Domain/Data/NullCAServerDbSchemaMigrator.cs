using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace CASecurity.Data;

/* This is used if database provider does't define
 * ICASecurityDbSchemaMigrator implementation.
 */
public class NullCASecurityDbSchemaMigrator : ICASecurityDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
