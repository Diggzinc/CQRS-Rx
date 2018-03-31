using System.Threading;
using System.Threading.Tasks;

namespace CQRSRx.Commands
{
    /// <summary>
    /// Asynchronous handler for commands.
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Handles a command asynchronously.
        /// </summary>
        /// <param name="command">The command to handle.</param>
        /// <returns>The correspondent task for the command handling.</returns>
        Task HandleAsync(ICommand command);
        
        /// <summary>
        /// Handles a command asynchronously with a custom cancellation token.
        /// </summary>
        /// <param name="command">The command to handle.</param>
        /// <param name="cancellationToken">The custom cancellation token.</param>
        /// <returns>The correspondent task for the command handling.</returns>
        Task HandleAsync(ICommand command, CancellationToken cancellationToken);
    }
}