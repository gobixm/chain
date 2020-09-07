using System.Threading;
using System.Threading.Tasks;

namespace Gobi.Chain.Core.Mediators.Handlers
{
    public class DelegateRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public delegate Task<TResponse> HandleFunc(TRequest request, CancellationToken ct);

        private readonly HandleFunc _handle;

        public DelegateRequestHandler(HandleFunc handle) => _handle = handle;

        public Task<TResponse> HandleAsync(TRequest request, CancellationToken ct) => _handle(request, ct);
    }
}
