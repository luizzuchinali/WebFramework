using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Luizanac.Infra.Http.Abstractions.Bindings
{
    /// <summary>
    /// Class that abstracts the use of a binded method with his arguments
    /// </summary>
    public class ActionBindInfo
    {
        /// <summary>
        /// The binded method
        /// </summary>
        public MethodInfo MethodInfo { get; private set; }

        /// <summary>
        /// Method arguments. If doesn't have any argument, this list will be empty
        /// </summary>
        public IReadOnlyCollection<HttpQueryArgument> Arguments { get; private set; }

        public bool HasArguments => Arguments.Any();

        public ActionBindInfo(MethodInfo methodInfo)
        {
            MethodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
            Arguments = new ReadOnlyCollection<HttpQueryArgument>(new List<HttpQueryArgument>());
        }

        public ActionBindInfo(MethodInfo methodInfo, IEnumerable<HttpQueryArgument> arguments)
        {
            MethodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));

            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));
            Arguments = new ReadOnlyCollection<HttpQueryArgument>(arguments.ToList());
        }


        /// <summary>
        /// Method responsible to invoke the binded action
        /// </summary>
        /// <param name="controllerInstance">The controller that has the method declared</param>
        /// <returns>Returns an <see cref="object"/> containing the action response</returns>
        public async Task<object> Invoke(object controllerInstance)
        {
            var isAwaitable = MethodInfo.ReturnType.GetMethod(nameof(Task.GetAwaiter)) != null;
            if (!HasArguments)
            {
                if (isAwaitable)
                    return await (dynamic)MethodInfo.Invoke(controllerInstance, new object[0]);
                else
                    return MethodInfo.Invoke(controllerInstance, new object[0]);
            }

            var methodParameters = MethodInfo.GetParameters();
            var invokeParameters = new object[methodParameters.Length];
            for (int i = 0; i < methodParameters.Length; i++)
            {
                var param = methodParameters[i];
                var argument = Arguments.Single(x => x.Key.Equals(param.Name, StringComparison.InvariantCultureIgnoreCase));

                invokeParameters[i] = Convert.ChangeType(argument.Value, param.ParameterType);
            }

            if (isAwaitable)
                return await (dynamic)MethodInfo.Invoke(controllerInstance, invokeParameters);
            else
                return MethodInfo.Invoke(controllerInstance, invokeParameters);
        }
    }
}