using System.Threading.Tasks;

namespace VCBRDemo.Data;

public interface IVCBRDemoDbSchemaMigrator
{
    Task MigrateAsync();
}
