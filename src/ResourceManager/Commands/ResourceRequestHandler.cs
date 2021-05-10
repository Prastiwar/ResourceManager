using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.Commands
{
    public abstract class ResourceRequestHandler<TQuery, TEnumerableQuery> : IRequestHandler<TQuery, object>,
                                                                             IRequestHandler<TEnumerableQuery, IEnumerable<object>>
        where TQuery : IResourceQuery
        where TEnumerableQuery : IEnumerableResourceQuery
    {
        public abstract Task<object> Handle(TQuery request, CancellationToken cancellationToken);

        public abstract Task<IEnumerable<object>> Handle(TEnumerableQuery request, CancellationToken cancellationToken);
    }
}
