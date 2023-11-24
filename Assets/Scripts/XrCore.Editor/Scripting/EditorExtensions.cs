using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using XrCore.EditorExceptions;

namespace XrCore.Scripting
{
    public static class EditorExtensions
    {
        #region Serialization extensiosn
        public static T GetValue<T>(this UnityEditor.SerializedProperty property)
        {
            object obj = property.serializedObject.targetObject;

            FieldInfo field = null;
            foreach (var path in property.propertyPath.Split('.'))
            {
                var type = obj.GetType();
                field = type.GetField(path, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (field == null)
                {
                    throw new InvalidSerializedPropertyException($"{path} property is invalid");
                }
                obj = field.GetValue(obj);
            }
            return (T)obj;
        }
        #endregion
    }
}