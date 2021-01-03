using System;

namespace Luizanac.Infra.IoC.Abstractions.Interfaces
{
    public interface IDIContainer
    {
        void Add(Type origin, Type destiny);
        void Add<TOrigin, TDestiny>() where TDestiny : TOrigin;
        object Get(Type origin);
    }
}