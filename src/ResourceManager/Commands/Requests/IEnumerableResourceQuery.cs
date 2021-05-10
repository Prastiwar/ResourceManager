using MediatR;
using System.Collections.Generic;
using System;

namespace ResourceManager.Commands
{
    public interface IEnumerableResourceQuery : IRequest<IEnumerable<object>>
    {
        Type ResourceType { get; }
    }
}
