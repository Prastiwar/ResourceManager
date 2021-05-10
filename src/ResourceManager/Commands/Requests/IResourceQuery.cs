using MediatR;
using System;

namespace ResourceManager.Commands
{
    public interface IResourceQuery : IRequest<object>
    {
        Type ResourceType { get; }
    }
}
