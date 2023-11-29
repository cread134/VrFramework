using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.DI
{
    internal class DependencyFactory
    {
        public static IDependency CreateDependency(DependencyType type, Type serviceType, Type resolutionType)
        {
            switch (type)
            {
                case DependencyType.Singleton:
                    return CreateSingletonDependency(serviceType, resolutionType);
                case DependencyType.Transient:
                    return CreateTransientDependency(serviceType, resolutionType);
            }
            throw new NotImplementedException();
        }

        static IDependency CreateSingletonDependency(Type serviceType, Type resolutionType)
        {
            bool isMonoBehaviour = resolutionType.IsMonoBehaviourDependency();

            return new SingletonDependency(serviceType, resolutionType, isMonoBehaviour);
        }

        static IDependency CreateTransientDependency(Type serviceType, Type resolutionType)
        {
            throw new NotImplementedException();
        }
    }
}
