using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.Commands
{
    /// <summary> Handles GetResourceByPath where path should be formatted as [TableName].ResourceId </summary>
    public class GetResourceByPathSqlHandler : ResourceRequestHandler<GetResourceByPathQuery, GetResourcesByPathQuery>
    {
        public GetResourceByPathSqlHandler(ISqlClient client) => Client = client;

        protected ISqlClient Client { get; }

        public override Task<object> Handle(GetResourceByPathQuery request, CancellationToken cancellationToken)
            => GetResourceByPath(request.ResourceType, request.Path);

        public override Task<IEnumerable<object>> Handle(GetResourcesByPathQuery request, CancellationToken cancellationToken)
        {
            FormatPath(request.Path, out string tableName, out string id);
            return Client.SelectAsync(tableName, request.ResourceType);
        }

        protected async Task<object> GetResourceByPath(Type resourceType, string path)
        {
            FormatPath(path, out string tableName, out string id);
            return await Client.SelectScalarAsync(tableName, resourceType, id);
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
