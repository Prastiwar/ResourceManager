using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ResourceManager.Common.Extensions
{
    public static class MediatorExtensions
    {
        public static async Task<T> SendAs<T>(this IMediator mediator, IBaseRequest request, CancellationToken cancellationToken = default)
        {
            object results = await mediator.Send(request, cancellationToken);
            return (T)results;
        }
    }
}
