using System;
using System.Collections.Generic;
using System.Linq;
using Luizanac.Infra.IoC.Abstractions.Interfaces;

namespace Luizanac.Infra.IoC
{
    //TODO: First implement a simple di container. After, add the possibility to add transient, scoped and singleton services
    public class DIContainer : IDIContainer
    {
        private readonly IDictionary<Type, Type> _types = new Dictionary<Type, Type>();

        public void Add(Type origin, Type destiny)
        {
            if (_types.ContainsKey(origin))
                throw new InvalidOperationException($"Type {origin.Name} alredy mapped");

            VerifyHierarchy(origin, destiny);

            _types.Add(origin, destiny);
        }

        private void VerifyHierarchy(Type origin, Type destiny)
        {
            if (origin.IsInterface)
            {
                var implementsInterface = destiny.GetInterfaces().Any(x => x.Equals(origin));
                if (!implementsInterface)
                    throw new InvalidOperationException($"{destiny} doesn't implements {origin.Name}");
            }
            else
                if (!destiny.IsSubclassOf(origin))
                throw new InvalidOperationException($"{destiny} doesn't is subclass of {origin}");
        }

        public object Get(Type origin)
        {
            if (_types.ContainsKey(origin))
            {
                var destiny = _types[origin];
                return Get(destiny);
            }

            var constructors = origin.GetConstructors();
            var constructorWithoutArguments = constructors.FirstOrDefault(x => x.GetParameters().Any() == false);
            if (constructorWithoutArguments != null)
                return constructorWithoutArguments.Invoke(new object[0]);

            var constructor = constructors[0];
            var constructorArguments = constructor.GetParameters();
            var arguments = new object[constructorArguments.Count()];
            for (int i = 0; i < constructorArguments.Count(); i++)
            {
                var argument = constructorArguments[i];
                var argumentType = argument.ParameterType;

                arguments[i] = Get(argumentType);
            }

            return constructor.Invoke(arguments);
        }
    }
}