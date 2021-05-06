using ResourceManager;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RPGDataEditor.Commands
{
    public class GetPresentableByIdSqlHandler<TResource> : GetPresentableByIdHandler<TResource>
    {
        public GetPresentableByIdSqlHandler(ISqlClient client) => Client = client;

        protected ISqlClient Client { get; }

        protected override async Task<TResource> GetResourceAsync(GetPresentableByIdQuery<TResource> request, CancellationToken cancellationToken)
        {
            string table = GetTableName();
            return (TResource)(await Client.SelectScalarAsync(table, typeof(TResource), request.Id));
        }

        protected override async Task ProcessResourcesAsync(IList<object> resources, GetPresentableByIdQuery<TResource> request, CancellationToken cancellationToken)
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
