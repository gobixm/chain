using System;
using System.Collections.Generic;
using Gobi.Chain.Core.Mediators.Middlewares;

namespace Gobi.Chain.Core.Mediators.Configurations
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
