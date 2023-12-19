using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
namespace Core.Logging
{
    public class LoggingService : ILoggingService
    {
        public LoggingService() 
        {
            Debug.Log("Created logging service");
        }

        public void Log(string message)
        {
#if UNITY_EDITOR 
            Debug.Log(message);
#endif

        }

        public void LogError(string message)
        {
#if UNITY_EDITOR 
            Debug.LogError(message);
#endif
        }

        public void Assert(bool condition, string message)
        {
#if UNITY_EDITOR 
            Debug.Assert(condition);
#endif
        }
    }
}