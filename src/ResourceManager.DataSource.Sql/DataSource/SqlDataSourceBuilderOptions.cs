using Microsoft.EntityFrameworkCore;
using ResourceManager.Services;

namespace ResourceManager.DataSource.Sql
{
    public class SqlDataSourceBuilderOptions : IDataSourceProviderBuilderOptions
    {
        public string Name { get; set; }

        public string ConnectionString { get; set; }

        public IResourceDescriptorService DescriptorService { get; set; }
        
        public DbContext DatabaseContext { get; set; }
    }
}