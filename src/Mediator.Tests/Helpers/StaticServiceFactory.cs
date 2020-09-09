using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Gobi.Chain.Mediator.Core.Resolvers;

namespace Gobi.Chain.Mediators.Tests.Helpers
{
    public sealed class StaticServiceFactory : IServiceFactory
    {
        private readonly ConcurrentDictionary<Type, object> _services = new ConcurrentDictionary<Type, object>();

        public T GetService<T>() => (T)_services.GetValueOrDefault(typeof(T));

        public StaticServiceFactory AddService<T>(T implementation)
        {
            _services.TryAdd(typeof(T), implementation);
            return this;
        }
    }
}
