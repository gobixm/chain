namespace Gobi.Chain.Core.Mediators.Resolvers
{
    public interface IServiceFactory
    {
        public T GetService<T>();
    }
}
