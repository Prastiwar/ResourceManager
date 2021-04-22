using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.Commands
{
    /// <summary> Handles GetResourceByPath where path should be formatted as [TableName].ResourceId </summary>
    public class GetResourceByPathSqlHandler<TResource> : GetResourceByPathHandler<TResource>
    {
        public GetResourceByPathSqlHandler(ISqlClient client) => Client = client;

        protected ISqlClient Client { get; }

        protected override async Task<TResource> GetResourceAsync(GetResourceByPathQuery<TResource> request, CancellationToken cancellationToken) => (TResource)await GetResourceByPath(request.Path);

        protected override async Task ProcessResourcesAsync(IList<object> resources, GetResourceByPathQuery<TResource> request, CancellationToken cancellationToken)
        {
            FormatPath(request.Path, out string tableName, out string id);
            IEnumerable<object> entities = await Client.SelectAsync(tableName, typeof(TResource));
            resources.AddRange(entities);
        }

        protected async Task<object> GetResourceByPath(string path)
        {
            FormatPath(path, out string tableName, out string id);
            return await Client.SelectScalarAsync(tableName, typeof(TResource), id);
        }

        protected virtual void FormatPath(string path, out string tableName, out string id)
        {
            try
            {
                tableName = path.Substring(0, path.LastIndexOf(']'));
                id = path.Substring(tableName.Length + 1);
            }
            catch (Exception ex)
            {
                throw new FormatException("Path to sql should have format [TableName].ResourceId, is: " + path, ex);
            }
        }
    }
}
