using System;

namespace Luizanac.Infra.IoC.Abstractions.Interfaces
{
    public interface IDIContainer
    {
        void Add(Type origin, Type destiny);
        object Get(Type origin);
    }
}