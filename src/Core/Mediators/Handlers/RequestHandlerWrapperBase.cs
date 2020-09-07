using System.Threading;
using System.Threading.Tasks;
using Gobi.Chain.Core.Mediators.Resolvers;

namespace Gobi.Chain.Core.Mediators.Handlers
{
    internal abstract class RequestHandlerWrapperBase
    {
        public abstract Task<object> HandleAsync(object request, CancellationToken ct,
            IServiceFactory serviceFactory);
    }
}
