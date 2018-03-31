
using System.Threading;
using System.Threading.Tasks;

namespace CQRSRx.Commands
{
    /// <summary>
    /// Defines the processor for commands.
    /// The CommandProcessor is responsible to route the commands to their respective handler.
    /// </summary>
    public interface ICommandProcessor
    {
        /// <summary>
        /// Routes the command to their respective handler asynchronously.
        /// </summary>
        /// <param name="command">The command to route.</param>
        /// <returns>The correspondent task for the command handling.</returns>
        Task ProcessAsync(ICommand command);

        /// <summary>
        /// Routes the command to their respective handler asynchronously with a custom cancellation token.
        /// </summary>
        /// <param name="command">The command to route.</param>
        /// <param name="cancellationToken">The custom cancellation token.</param>
        /// <returns>The correspondent task for the command handling.</returns>
        Task ProcessAsync(ICommand command, CancellationToken cancellationToken);
    }
}