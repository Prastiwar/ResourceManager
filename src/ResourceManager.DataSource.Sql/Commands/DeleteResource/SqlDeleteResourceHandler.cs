﻿using ResourceManager.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.DataSource.Sql.Commands
{
    public class SqlDeleteResourceHandler : DeleteResourceHandler
    {
        public override Task<DeleteResourceResults> Handle(DeleteResourceRequest request, CancellationToken cancellationToken) => throw new System.NotImplementedException();
    }
}
