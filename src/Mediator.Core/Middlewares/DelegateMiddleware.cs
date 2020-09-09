using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gobi.Chain.Mediator.Core.Middlewares
{
    public sealed class DelegateMiddleware<T> : IMiddleware<T> where T : new()
    {
        private readonly Func<T, NextMiddlewareDelegate, CancellationToken, Task> _handle;

        public DelegateMiddleware(Func<T, NextMiddlewareDelegate, CancellationToken, Task> handle) => _handle = handle;

        public Task HandleAsync(T context, NextMiddlewareDelegate next, CancellationToken ct) =>
            _handle(context, next, ct);
    }
}
