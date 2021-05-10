using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.Commands
{
    public class GetResourceByIdSqlHandler : ResourceRequestHandler<GetResourceByIdQuery, GetResourcesByIdQuery>
    {
        public GetResourceByIdSqlHandler(ISqlClient client) => Client = client;

        protected ISqlClient Client { get; }

        public override async Task<object> Handle(GetResourceByIdQuery request, CancellationToken cancellationToken)
        {
            string table = GetTableName(request.ResourceType);
            return await Client.SelectScalarAsync(table, request.ResourceType, request.Id);
        }

        public override Task<IEnumerable<object>> Handle(GetResourcesByIdQuery request, CancellationToken cancellationToken)
        {
            string table = GetTableName(request.ResourceType);
            return Client.SelectAsync(table, request.ResourceType);
        }

        private string GetTableName(Type resourceType) => BraceTableName(Client.GetTableName(resourceType));

        private string BraceTableName(string table)
        {
            if (!table.StartsWith('['))
            {
                table = '[' + table;
            }
            if (!table.EndsWith(']'))
            {
                table += ']';
            }
            return table;
        }
    }
}
