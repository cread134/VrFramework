using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.DI
{
    public class DependencyConfiguration
    {
        public DependencyConfiguration(ComponentConfiguration componentConfiguration) 
        {
            ComponentConfiguration = componentConfiguration;
        }

        public ComponentConfiguration ComponentConfiguration { get; }
    }

    public class ComponentConfiguration
    {
        public static ComponentConfiguration Empty = new ComponentConfiguration();
        private List<Type> _types;

        public List<Type> Types => _types;

        public static ComponentConfiguration FromTypes(params Type[] types)
        {
            var instance = new ComponentConfiguration();
            foreach (var type in types)
            {
                instance.RegisterTypeCore(type);
            }
            return instance;
        }

        private void RegisterTypeCore(Type rawType)
        {

        }

        public void RegisterType<T>() where T : MonoBehaviour
        {

        }
        
        public ComponentConfiguration()
        {
            _types = new List<Type>();
        }
    }
}