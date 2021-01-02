using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Luizanac.Infra.Http.Abstractions.Bindings;

namespace Luizanac.Infra.Http.Bindings
{
    /// <summary>
    /// Class responsible to get a method and bind his arguments
    /// </summary>
    public class ActionBinder : IActionBinder
    {
        /// <summary>
        /// Method responsible to bind a method with his arguments
        /// </summary>
        /// <param name="controllerInstance">The controller instance to get the method</param>
        /// <param name="actionName">The method name</param>
        /// <param name="path">Url path to get query params</param>
        /// <returns>Returns an <see cref="ActionBindInfo"/> containing the binded method</returns>
        public ActionBindInfo BindAction(object controllerInstance, string actionName, string path)
        {
            var queryStringIndicator = path.IndexOf('?');
            var hasQuery = queryStringIndicator >= 0;

            if (!hasQuery)
                return new ActionBindInfo(controllerInstance.GetType().GetMethod(actionName));
            else
            {
                var queryString = path.Substring(queryStringIndicator + 1);
                var arguments = GetArguments(queryString);
                var argumentNames = arguments.Select(x => x.Key.ToLowerInvariant()).ToArray();
                return new ActionBindInfo(GetMethodInfo(actionName, argumentNames, controllerInstance), arguments);
            }
        }

        /// <summary>
        /// Get the arguments from querystring
        /// </summary>
        /// <param name="queryString">A querystring containing key values</param>
        /// <returns>Returns a <see cref="IEnumerable"/> of <see cref="HttpQueryArgument"/></returns>
        private IEnumerable<HttpQueryArgument> GetArguments(string queryString)
        {
            var tuples = queryString.Split('&', StringSplitOptions.RemoveEmptyEntries);
            foreach (var tuple in tuples)
            {
                var keyValue = tuple.Split('=', StringSplitOptions.RemoveEmptyEntries);
                yield return new HttpQueryArgument(keyValue[0], keyValue[1]);
            }
        }

        /// <summary>
        /// Responsible to get the correct method
        /// </summary>
        /// <param name="actionName">The name of method do bind</param>
        /// <param name="argumentNames">Array of arguments</param>
        /// <param name="controllerInstance">The controller instance where the method is declared</param>
        /// <returns>Returns a <see cref="MethodInfo"/></returns>
        /// <exception cref="ArgumentException">If a valid method has not founded</exception>
        private MethodInfo GetMethodInfo(string actionName, string[] argumentNames, object controllerInstance)
        {
            var bindingFlags =
                BindingFlags.Instance |
                BindingFlags.Static |
                BindingFlags.Public |
                BindingFlags.IgnoreCase |
                BindingFlags.DeclaredOnly;

            var methods = controllerInstance.GetType().GetMethods(bindingFlags).Where(x => x.Name.Equals(actionName, StringComparison.InvariantCultureIgnoreCase));
            foreach (var method in methods)
            {
                var parameters = method.GetParameters();
                var match = parameters.All(x => argumentNames.Contains(x.Name.ToLowerInvariant()));
                if (match)
                    return method;
            }

            throw new ArgumentException($"Doesn't have an implement method called {actionName}");
        }
    }
}