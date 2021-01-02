using System.Reflection;

namespace Luizanac.Infra.Http.Abstractions.Bindings
{
    /// <summary>
    /// Interface responsible to get a method and bind his arguments
    /// </summary>
    public interface IActionBinder
    {
        /// <summary>
        /// Method responsible to bind a method with his arguments
        /// </summary>
        /// <param name="controllerInstance">The controller instance to get the method</param>
        /// <param name="actionName">The method name</param>
        /// <param name="path">Url path to get query params</param>
        /// <returns>Returns an <see cref="ActionBindInfo"/> containing the binded method</returns>
        ActionBindInfo BindAction(object controllerInstance, string actionName, string path);
    }
}