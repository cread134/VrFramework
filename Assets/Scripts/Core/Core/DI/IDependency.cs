using System;

namespace Core.DI
{
    public interface IDependency
    {
        public Type ServiceType { get; }
        public Type DependencyBaseType { get; }
        public bool IsMonoBehaviour { get; }

        public T GetInstance<T>();

        public IDependency OnBuild();
        public void Configure(DependencyConfiguration config);
    }
}