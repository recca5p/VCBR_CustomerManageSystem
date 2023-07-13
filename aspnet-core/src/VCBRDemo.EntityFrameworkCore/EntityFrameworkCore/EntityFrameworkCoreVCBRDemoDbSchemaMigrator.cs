using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VCBRDemo.Data;
using Volo.Abp.DependencyInjection;

namespace VCBRDemo.EntityFrameworkCore;

public class EntityFrameworkCoreVCBRDemoDbSchemaMigrator
    : IVCBRDemoDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreVCBRDemoDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the VCBRDemoDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<VCBRDemoDbContext>()
            .Database
            .MigrateAsync();
    }
}
