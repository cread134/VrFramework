using EditorTools.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace EditorTools.Scripting
{
    public static class EditorExtensions
    {
        #region Serialization extensions
        public static T GetValue<T>(this UnityEditor.SerializedProperty property)
        {
            object obj = property.serializedObject.targetObject;
            var type = obj.GetType();
            var types = new List<Type>
            {
                type
            };
            if (type.BaseType != null && type.BaseType != typeof(MonoBehaviour)) 
            {
                types.Add(type.BaseType);
            }

            FieldInfo field = null;
            foreach (var path in property.propertyPath.Split('.'))
            {
                foreach (var t in types)
                {
                    field ??= t.GetField(path, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
                Debug.Assert(field != null, $"{path} does not have associated field");
                obj = field?.GetValue(obj);
            }
            return (T)obj;
        }
        #endregion
    }
}