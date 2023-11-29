using Core.DI;
using Core.Logging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.ApplicationContext
{
    public class ApplicationContext
    {
        [RuntimeInitializeOnLoadMethod]
        public static void ProgramStart()
        {
            Debug.Log("Initialize Game app");
            BuildDIContext();

            var logginService = DependencyService.Resolve<ILoggingService>();
        }

        static void BuildDIContext()
        {
            Debug.Log("Building DI context");
            var container = DependencyService.BuildContainer();
            container.RegisterService<ILoggingService, LoggingService>(DependencyType.Singleton);
            container.Build();
        }
    }
}
