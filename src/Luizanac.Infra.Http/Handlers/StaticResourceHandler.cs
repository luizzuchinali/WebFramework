using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Luizanac.Infra.Http.Abstractions;
using Luizanac.Infra.Http.Abstractions.Handlers;

namespace Luizanac.Infra.Http.Handlers
{
    /// <summary>
    /// Provide methods to manipulate <see cref="HttpListenerContext"/>. Focused to work with static resources.
    /// </summary>
    public class StaticResourceHandler : IAsyncHttpHandler
    {
        public HttpListenerContext HttpContext { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticResourceHandler" /> class.
        /// </summary>
        /// <param name="httpContext"><see cref="HttpListenerContext" /></param>
        public StaticResourceHandler(HttpListenerContext httpContext)
        {
            HttpContext = httpContext;
        }

        /// <summary>
        /// Process the request to make the decisions regarding which static resource will be returned.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to abort the process</param>
        /// <returns>Returns a <see cref="Task"/> representing the async process</returns>
        public async Task HandleAsync(CancellationToken cancellationToken = default)
        {
            var path = HttpContext.Request.Url.AbsolutePath;
            var assembly = Assembly.GetExecutingAssembly();

            var publicDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultDirectories.Public, path.Remove(0, 1));
            if (!File.Exists(publicDirectory))
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return;
            }

            var buffer = await File.ReadAllBytesAsync(publicDirectory, cancellationToken);
            HttpContext.Response.ContentType = path.ResolveContentType();
            HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            HttpContext.Response.ContentLength64 = buffer.Length;
            await HttpContext.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length, cancellationToken);

            HttpContext.Response.OutputStream.Close();
        }
    }
}