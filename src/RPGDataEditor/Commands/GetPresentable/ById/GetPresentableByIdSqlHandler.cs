using ResourceManager;
using ResourceManager.Services;
using RPGDataEditor.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RPGDataEditor.Commands
{
    public class GetPresentableByIdSqlHandler : GetPresentableHandler<GetPresentableByIdQuery, GetPresentablesByIdQuery>
    {
        public GetPresentableByIdSqlHandler(IResourceDescriptorService descriptorService, ISqlClient client)
            : base(descriptorService) => Client = client;

        protected ISqlClient Client { get; }

        public override Task<PresentableData> Handle(GetPresentableByIdQuery request, CancellationToken cancellationToken)
        {
            string table = GetTableName(request.ResourceType);
            return GetPresentableByPath(request.ResourceType, table + "." + request.Id);
        }

        public override async Task<IEnumerable<PresentableData>> Handle(GetPresentablesByIdQuery request, CancellationToken cancellationToken)
        {
            string table = GetTableName(request.ResourceType);
            IList<PresentableData> presentables = new List<PresentableData>();
            List<Exception> exceptions = new List<Exception>();
            foreach (object id in request.Ids)
            {
                try
                {
                    PresentableData presentable = await GetPresentableByPath(request.ResourceType, table + "." + id);
                    if (presentable != null)
                    {
                        presentables.Add(presentable);
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }
            if (exceptions.Count > 0)
            {
                throw new AggregateException("One or more presentables threw error while retrieving them", exceptions);
            }
            return presentables;
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
