using System.Threading;
using System.Threading.Tasks;

namespace Gobi.Chain.Core.Mediators.Handlers
{
    public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public Task<TResponse> HandleAsync(TRequest request, CancellationToken ct);
    }
}
