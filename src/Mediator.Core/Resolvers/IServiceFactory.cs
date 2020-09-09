namespace Gobi.Chain.Mediator.Core.Resolvers
{
    public interface IServiceFactory
    {
        public T GetService<T>();
    }
}
