using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.DI
{
    public class DependencyContainer
    {
        private Dictionary<Type, IDependency> _dependencyRegistry;

        public DependencyContainer()
        {
            _dependencyRegistry = new Dictionary<Type, IDependency>();
        }

        public IDependency RegisterService<T>(DependencyType dependencyType)
        {
            return RegisterService<T, T>(dependencyType);
        }

        public IDependency RegisterService<T, U>(DependencyType dependencyType)
        {
            if(_dependencyRegistry.ContainsKey(typeof(T)))
            {
                throw new InvalidOperationException("Type is already registered");
            }
            var createdDependency = DependencyFactory.CreateDependency(dependencyType, typeof(T), typeof(U));
            _dependencyRegistry.Add(typeof(T), createdDependency);
            return createdDependency;
        }

        public T Resolve<T>()
        {
            if (_dependencyRegistry.ContainsKey(typeof(T)))
            {
                return _dependencyRegistry[typeof(T)].GetInstance<T>();
            }
            throw new InvalidOperationException($"Could not resolve dependecy of type {typeof(T).Name}");
        }

        public void Build()
        {
            foreach (var dependency in _dependencyRegistry.Values)
            {
                dependency.OnBuild();
            }
            Debug.Log("Built dependency container");
        }
    }
}