using System.Threading;
using System.Threading.Tasks;
using Gobi.Chain.Mediator.Core.Resolvers;

namespace Gobi.Chain.Mediator.Core.Handlers
{
    internal abstract class RequestHandlerWrapperBase
    {
        public abstract Task<object> HandleAsync(object request, CancellationToken ct,
            IServiceFactory serviceFactory);
    }
}
