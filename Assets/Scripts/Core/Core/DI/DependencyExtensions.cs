using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Core.DI
{
    public static class DependencyExtensions
    {
        public static bool IsMonoBehaviourDependency(this Type service)
        {
            var baseTypes = service.GetParentTypes();
            return baseTypes.Contains(typeof(MonoBehaviour));
        }

        public static IEnumerable<Type> GetParentTypes(this Type type)
        {
            if (type == null)
            {
                yield break;
            }

            foreach (var i in type.GetInterfaces())
            {
                yield return i;
            }

            var currentBaseType = type.BaseType;
            while (currentBaseType != null)
            {
                yield return currentBaseType;
                currentBaseType = currentBaseType.BaseType;
            }
        }
    }
}