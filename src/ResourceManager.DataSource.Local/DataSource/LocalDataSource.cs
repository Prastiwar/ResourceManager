using Microsoft.Extensions.Configuration;
using ResourceManager.DataSource.Local.Configuration;
using ResourceManager.Services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.Local
{
    public class LocalDataSource : DataSource
    {
        public LocalDataSource(IConfiguration configuration, IConnectionMonitor monitor, IResourceDescriptorService descriptorService, LocalDataSourceOptions options)
        {
            Configuration = configuration;
            Monitor = monitor;
            DescriptorService = descriptorService;
            Options = options;
        }

        public LocalDataSourceOptions Options { get; }

        public override void SaveChanges() => throw new NotImplementedException();

        public override Task SaveChangesAsync(CancellationToken token) => throw new NotImplementedException();

        public override IQueryable<object> Query(Type resourceType) => throw new NotImplementedException();

        public override IQueryable<string> Locate(Type resourceType) => throw new NotImplementedException();
    }
}
