using System;
using System.Collections;
using System.Collections.Generic;
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
            throw new System.NotImplementedException();
        }

        public void LogError(string message)
        {
            throw new System.NotImplementedException();
        }
    }
}