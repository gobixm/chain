﻿using System.Threading;
using System.Threading.Tasks;
using Gobi.Chain.Mediator.Core.Resolvers;

namespace Gobi.Chain.Mediator.Core.Handlers
{
    internal sealed class RequestRequestHandlerWrapper<TRequest, TResponse> : RequestHandlerWrapperBase
        where TRequest : IRequest<TResponse>
    {
        public override async Task<object> HandleAsync(object request, CancellationToken ct,
            IServiceFactory serviceFactory) => await HandleAsync((TRequest)request, ct, serviceFactory);

        public Task<TResponse> HandleAsync(TRequest request, CancellationToken ct, IServiceFactory serviceFactory) =>
            serviceFactory.GetService<IRequestHandler<TRequest, TResponse>>().HandleAsync(request, ct);
    }
}
