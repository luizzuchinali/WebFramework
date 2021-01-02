using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Luizanac.Infra.Http.Abstractions.Handlers;

namespace Luizanac.Infra.Http.Manipulators
{
    /// <summary>
    /// Provide methods to manipulate <see cref="HttpListenerContext"/>. Focused to work with controllers.
    /// </summary>
    public class ControllerHandler : IAsyncHttpHandler
    {
        public HttpListenerContext HttpContext { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerHandler" /> class.
        /// </summary>
        /// <param name="httpContext"><see cref="HttpListenerContext" /></param>
        public ControllerHandler(HttpListenerContext httpContext)
        {
            HttpContext = httpContext;
        }

        /// <summary>
        /// Process the request to make the decisions regarding which controller and action will be executed.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to abort the process</param>
        /// <returns>Returns a <see cref="Task"/> representing the async process</returns>
        public async Task HandleAsync(CancellationToken cancellationToken = default)
        {
            var path = HttpContext.Request.Url.AbsolutePath;

            var controllerName = path.ExtractControllerName();
            var actionName = path.ExtractActionName();
            var controllerTypesQuery = Assembly.GetEntryAssembly().GetTypes().Where(x => x.IsPublic && !x.IsAbstract && x.IsSubclassOf(typeof(ControllerBase)));
            controllerTypesQuery = controllerTypesQuery.Where(x => x.Name.Replace("Controller", "").Equals(controllerName, StringComparison.InvariantCultureIgnoreCase)).ToArray();

            if (controllerTypesQuery.Count() > 1)
                throw new AmbiguousMatchException($"Cannot have another controller with this name {controllerName}");

            var controllerType = controllerTypesQuery.SingleOrDefault();
            if (controllerType is not null)
            {
                var controllerInstance = Activator.CreateInstance(controllerType);
                var action = controllerInstance.GetType().GetMethod(actionName);
                if (action is not null)
                {
                    var isAwaitable = action.ReturnType.GetMethod(nameof(Task.GetAwaiter)) != null;

                    object result = null;
                    if (isAwaitable)
                        result = await (dynamic)action.Invoke(controllerInstance, new object[] { });
                    else
                        result = action.Invoke(controllerInstance, new object[] { });

                    var resourceBuffer = Encoding.UTF8.GetBytes((string)result);
                    HttpContext.Response.ContentType = "text/html; charset=utf-8";//path.ResolveContentType();
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                    HttpContext.Response.ContentLength64 = resourceBuffer.Length;
                    await HttpContext.Response.OutputStream.WriteAsync(resourceBuffer, 0, resourceBuffer.Length, cancellationToken);
                }
                else
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                }
            }
            else
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }

            HttpContext.Response.OutputStream.Close();
        }
    }
}