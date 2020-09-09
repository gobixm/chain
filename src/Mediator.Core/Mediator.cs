using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gobi.Chain.Mediator.Core.Configurations;
using Gobi.Chain.Mediator.Core.Handlers;
using Gobi.Chain.Mediator.Core.Middlewares;
using Gobi.Chain.Mediator.Core.Resolvers;

namespace Gobi.Chain.Mediator.Core
{
    public sealed class Mediator<T> where T : new()
    {
        private readonly MediatorConfiguration<T> _configuration;
        private readonly ConcurrentDictionary<Type, object> _handlerWrappers = new ConcurrentDictionary<Type, object>();
        private readonly IServiceFactory _serviceFactory;

        public Mediator(IServiceFactory serviceFactory, Action<MediatorConfiguration<T>> configure = null)
        {
            _serviceFactory = serviceFactory;
            MediatorConfiguration<T> config = new MediatorConfiguration<T>();
            configure?.Invoke(config);
            _configuration = config;
        }

        public async Task<TResponse> RequestAsync<TResponse>(IRequest<TResponse> request,
            CancellationToken ct = default)
        {
            object handlerWrapper = _handlerWrappers.GetOrAdd(request.GetType(), requestType =>
                Activator.CreateInstance(
                    typeof(RequestRequestHandlerWrapper<,>).MakeGenericType(requestType, typeof(TResponse))));

            T context = new T();
            return await _configuration.MiddlewareFactories
                .Reverse()
                .Aggregate<Func<IMiddleware<T>>, Func<IRequest<TResponse>, Task<TResponse>>>(
                    async req => (TResponse)await ((RequestHandlerWrapperBase)handlerWrapper).HandleAsync(req, ct,
                        _serviceFactory),
                    (next, factory) => async req =>
                    {
                        TResponse result = default;
                        await factory().HandleAsync(context, async () => result = await next(req), ct);
                        return result;
                    })(request);
        }
    }
}
