using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.Commands
{
    public class GetResourceByIdSqlHandler<TResource> : GetResourceByIdHandler<TResource>
    {
        public GetResourceByIdSqlHandler(ISqlClient client) => Client = client;

        protected ISqlClient Client { get; }

        protected override async Task<TResource> GetResourceAsync(GetResourceByIdQuery<TResource> request, CancellationToken cancellationToken)
        {
            string table = GetTableName();
            return (TResource)(await Client.SelectScalarAsync(table, typeof(TResource), request.Id));
        }

        protected override async Task ProcessResourcesAsync(IList<object> resources, GetResourceByIdQuery<TResource> request, CancellationToken cancellationToken)
        {
            string table = GetTableName();
            IEnumerable<object> entities = await Client.SelectAsync(table, typeof(TResource).GetEnumerableElementType());
            resources.AddRange(entities);
        }

        private string GetTableName() => BraceTableName(Client.GetTableName(typeof(TResource)));

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
