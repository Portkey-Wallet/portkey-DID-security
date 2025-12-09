using System.Threading.Tasks;

namespace CASecurity.Data;

public interface ICASecurityDbSchemaMigrator
{
    Task MigrateAsync();
}
