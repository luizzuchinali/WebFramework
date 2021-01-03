using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Luizanac.Infra.Http.Abstractions.Handlers;
using Luizanac.Infra.Http.Handlers;
using Luizanac.Infra.IoC;
using Luizanac.Infra.IoC.Abstractions.Interfaces;

namespace Luizanac.Infra.Http
{

    /// <summary>
    /// Class to bootstrap the application and IoC container
    /// </summary>
    public class WebApplication
    {
        private readonly string[] _routePrefixes;
        private static readonly IDIContainer _container = new DIContainer();
        private readonly HttpListener _httpListener = new HttpListener();

        public WebApplication(string[] routePrefixes)
        {
            _ = routePrefixes ?? throw new ArgumentNullException(nameof(routePrefixes));
            _routePrefixes = routePrefixes;

            foreach (var routePrefix in _routePrefixes)
                _httpListener.Prefixes.Add(routePrefix);
        }

        /// <summary>
        /// Used to add some dependencies to container
        /// </summary>
        /// <param name="action">An <see cref="Action"/> of <see cref="IDIContainer"/></param>
        public void Configure(Action<IDIContainer> action)
        {
            action.Invoke(_container);
        }

        /// <summary>
        /// Initilize the application server
        /// </summary>
        public void Init()
        {
            _httpListener.Start();
            while (true)
            {
                var result = _httpListener.BeginGetContext(async (IAsyncResult res) => await OnContext(res), _httpListener);
                result.AsyncWaitHandle.WaitOne();
            }
        }

        public async Task OnContext(IAsyncResult result)
        {
            var context = _httpListener.EndGetContext(result);

            IAsyncHttpHandler handler;
            if (context.Request.Url.AbsolutePath.IsStaticResource())
                handler = new StaticResourceHandler(context);
            else
                handler = new ControllerHandler(context, _container);

            await handler.HandleAsync();
        }
    }
}