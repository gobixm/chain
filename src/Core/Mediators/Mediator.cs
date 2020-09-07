using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Gobi.Chain.Core.Mediators.Configurations;
using Gobi.Chain.Core.Mediators.Handlers;
using Gobi.Chain.Core.Mediators.Resolvers;

namespace Gobi.Chain.Core.Mediators
{
    public sealed class Mediator
    {
        private readonly ConcurrentDictionary<Type, object> _handlerWrappers = new ConcurrentDictionary<Type, object>();
        private readonly IServiceFactory _serviceFactory;

        public Mediator(IServiceFactory serviceFactory, Action<MediatorConfiguration> configure = null)
        {
            _serviceFactory = serviceFactory;
            MediatorConfiguration configuration = new MediatorConfiguration();
            configure?.Invoke(configuration);
        }

        public async Task<TResponse> RequestAsync<TResponse>(IRequest<TResponse> request,
            CancellationToken ct = default)
        {
            object handlerWrapper = _handlerWrappers.GetOrAdd(request.GetType(), requestType =>
                Activator.CreateInstance(
                    typeof(RequestRequestHandlerWrapper<,>).MakeGenericType(requestType, typeof(TResponse))));

            object response = await ((RequestHandlerWrapperBase)handlerWrapper).HandleAsync(request, ct, _serviceFactory);

            return (TResponse)response;
        }
    }
}
