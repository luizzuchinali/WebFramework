using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Luizanac.Infra.Http.Abstractions.Handlers;
using Luizanac.Infra.Http.Handelrs;
using Luizanac.Infra.Http.Manipulators;

namespace Luizanac.Infra.Http
{

    /// <summary>
    /// Class to bootstrap the application and IoC container
    /// </summary>
    public class WebApplication
    {
        private readonly string[] _routePrefixes;

        public WebApplication(string[] routePrefixes)
        {
            _ = routePrefixes ?? throw new ArgumentNullException(nameof(routePrefixes));
            _routePrefixes = routePrefixes;
        }

        /// <summary>
        /// Initilize the application server
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to abort the process</param>
        /// <returns>Returns a <see cref="Task" /> representing the async process.</returns>
        public async Task InitAsync(CancellationToken cancellationToken = default)
        {
            while (true & !cancellationToken.IsCancellationRequested)
                await ApplicationLoop();
        }

        private async Task ApplicationLoop(CancellationToken cancellationToken = default)
        {
            var httpListener = new HttpListener();
            foreach (var routePrefix in _routePrefixes)
                httpListener.Prefixes.Add(routePrefix);

            httpListener.Start();

            var context = await httpListener.GetContextAsync();

            IAsyncHttpHandler handler;
            if (context.Request.Url.AbsolutePath.IsStaticResource())
                handler = new StaticResourceHandler(context);
            else
                handler = new ControllerHandler(context);

            await handler.HandleAsync();

            httpListener.Stop();
        }
    }
}