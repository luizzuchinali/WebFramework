using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Luizanac.Infra.Http.Abstractions.Handlers
{
    /// <summary>
    /// Provide methods to manipulate <see cref="HttpListenerContext"/>
    /// </summary>
    public interface IAsyncHttpHandler
    {
        /// <summary>
        /// The <see cref="HttpListenerContext"/> property
        /// </summary>
        HttpListenerContext HttpContext { get; }

        /// <summary>
        /// Handle the http request/response
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to abort the process</param>
        /// <returns>Returns a <see cref="Task"/> representing the async process</returns>
        Task HandleAsync(CancellationToken cancellationToken = default);
    }
}