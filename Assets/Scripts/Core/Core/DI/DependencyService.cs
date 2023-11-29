using System.Collections;
using System.Collections.Generic;

namespace Core.DI
{
    public enum DependencyType
    {
        Singleton,
        Transient
    }

    public class DependencyService
    {
        public static DependencyContainer DependencyContainer { get; private set; }
        public static DependencyContainer BuildContainer()
        {
            var createdContainer = new DependencyContainer();
            DependencyContainer = createdContainer;
            return createdContainer;
        }

        public static T Resolve<T>()
        {
            return DependencyContainer.Resolve<T>();
        }
    }
}
