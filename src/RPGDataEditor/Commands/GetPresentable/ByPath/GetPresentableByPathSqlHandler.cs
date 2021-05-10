using ResourceManager;
using ResourceManager.Services;
using RPGDataEditor.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RPGDataEditor.Commands
{
    /// <summary> Handles GetPresentableByPath where path should be formatted as [TableName].ResourceId </summary>
    public class GetPresentableByPathSqlHandler<TResource> : GetPresentableByPathHandler<TResource>
    {
        public GetPresentableByPathSqlHandler(ISqlClient client, IResourceDescriptorService descriptorService)
            : base(descriptorService) => Client = client;

        protected ISqlClient Client { get; }

        protected override async Task<PresentableData> GetResourceAsync(GetPresentableByPathQuery<TResource> request, CancellationToken cancellationToken) 
            => await GetPresentableByPath(request.Path);

        protected override async Task ProcessResourcesAsync(IList<object> resources, GetPresentableByPathQuery<TResource> request, CancellationToken cancellationToken)
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
