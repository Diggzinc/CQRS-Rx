using CQRSRx.Commands;
using CQRSRx.Queries;

namespace CQRSRx
{
    /// <summary>
    /// Generic CQS processor/router.
    /// </summary>
    public interface IProcessor : ICommandProcessor, IQueryProcessor { }
}