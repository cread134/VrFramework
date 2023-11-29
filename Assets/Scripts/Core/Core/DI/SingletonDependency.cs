using System;
using UnityEngine;

namespace Core.DI
{
    public class SingletonDependency : IDependency
    {
        public Type ServiceType { get; }
        public Type DependencyBaseType { get; }
        public bool IsMonoBehaviour { get; }

        private object _objectInstance;

        public SingletonDependency(Type serviceType, Type resolutionType, bool isMonoBehaviour) 
        {
            ServiceType = serviceType;
            DependencyBaseType = resolutionType;
            IsMonoBehaviour = isMonoBehaviour;

            Debug.Log($"Registered service: {resolutionType.Name} \n monobehaviour:{isMonoBehaviour}");
        }

        public void Configure(DependencyConfiguration configuration)
        {
            throw new NotImplementedException();
        }


        public T GetInstance<T>()
        {
            return ((T)_objectInstance) ?? throw new InvalidOperationException("Singleton dependency instance is missing");
        }

        public IDependency OnBuild()
        {
            if (IsMonoBehaviour)
            {
                throw new NotImplementedException();
            } 
            else
            {
                _objectInstance = Activator.CreateInstance(DependencyBaseType);
            }
            return this;
        }
    }
}