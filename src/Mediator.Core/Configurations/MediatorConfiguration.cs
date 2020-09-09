using System;
using System.Collections.Generic;
using Gobi.Chain.Mediator.Core.Middlewares;

namespace Gobi.Chain.Mediator.Core.Configurations
{
    public sealed class MediatorConfiguration<T> where T : new()
    {
        private readonly List<Func<IMiddleware<T>>> _middlewareFactories = new List<Func<IMiddleware<T>>>();

        public IReadOnlyCollection<Func<IMiddleware<T>>> MiddlewareFactories => _middlewareFactories;

        public MediatorConfiguration<T> AddMiddleware(Func<IMiddleware<T>> factory)
        {
            _middlewareFactories.Add(factory);
            return this;
        }
    }
}
