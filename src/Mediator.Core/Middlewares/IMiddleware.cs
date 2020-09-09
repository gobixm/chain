using System.Threading;
using System.Threading.Tasks;

namespace Gobi.Chain.Mediator.Core.Middlewares
{
    public delegate Task NextMiddlewareDelegate();

    public interface IMiddleware<in T> where T : new()
    {
        public Task HandleAsync(T context, NextMiddlewareDelegate next, CancellationToken ct = default);
    }
}
