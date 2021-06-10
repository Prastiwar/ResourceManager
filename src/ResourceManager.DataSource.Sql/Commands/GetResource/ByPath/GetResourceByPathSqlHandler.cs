using ResourceManager;
using ResourceManager.Commands;
using ResourceManager.Data;
using ResourceManager.DataSource.Sql.Data;
using ResourceManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.Sql.Commands
{
    public class GetResourceByPathSqlHandler : ResourceRequestHandler<GetResourceByPathQuery, GetResourcesByPathQuery>
    {
        public GetResourceByPathSqlHandler(IResourceDescriptorService descriptorService, ISqlClient client)
        {
            DescriptorService = descriptorService;
            Client = client;
        }

        protected IResourceDescriptorService DescriptorService { get; }

        protected ISqlClient Client { get; }

        public override Task<object> Handle(GetResourceByPathQuery request, CancellationToken cancellationToken)
            => GetResourceByPath(request.ResourceType, request.Path);

        public override async Task<IEnumerable<object>> Handle(GetResourcesByPathQuery request, CancellationToken cancellationToken)
        {
            string tableName = Client.GetTableName(request.ResourceType);
            IList<object> ids = new List<object>();
            SqlLocationResourceDescriptor pathDescriptor = DescriptorService.GetRequiredDescriptor<SqlLocationResourceDescriptor>(request.ResourceType);
            foreach (string path in request.Paths)
            {
                KeyValuePair<string, object>[] parameters = pathDescriptor.ParseParameters(path);
                KeyValuePair<string, object> idParameter = parameters.FirstOrDefault(parameter => string.Compare(parameter.Key, "id", true) == 0);
                if (idParameter.Key != null)
                {
                    ids.Add(idParameter.Value);
                }
            }
            return await Client.SelectAsync(tableName, request.ResourceType, ids.ToArray());
        }

        protected async Task<object> GetResourceByPath(Type resourceType, string path)
        {
            string tableName = Client.GetTableName(resourceType);
            SqlLocationResourceDescriptor pathDescriptor = DescriptorService.GetRequiredDescriptor<SqlLocationResourceDescriptor>(resourceType);
            KeyValuePair<string, object>[] parameters = pathDescriptor.ParseParameters(path);
            KeyValuePair<string, object> idParameter = parameters.FirstOrDefault(parameter => string.Compare(parameter.Key, "id", true) == 0);
            if (idParameter.Key != null)
            {
                return await Client.SelectScalarAsync(tableName, resourceType, idParameter.Value);
            }
            return default;
        }
    }
}
