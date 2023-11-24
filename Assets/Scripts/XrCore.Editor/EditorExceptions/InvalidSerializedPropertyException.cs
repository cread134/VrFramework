using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XrCore.EditorExceptions
{
    public class InvalidSerializedPropertyException : Exception
    {
        public InvalidSerializedPropertyException() { }
        public InvalidSerializedPropertyException(string message) : base(message) { }
        public InvalidSerializedPropertyException(string message, System.Exception inner) : base(message, inner) { }
        protected InvalidSerializedPropertyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
