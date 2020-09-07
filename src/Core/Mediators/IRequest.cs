namespace Gobi.Chain.Core.Mediators
{
    /// <summary>
    /// Request interface.
    /// </summary>
    /// <typeparam name="TResponse">Coupling with response. Used for type inference.</typeparam>
    public interface IRequest<out TResponse>
    {
    }
}
