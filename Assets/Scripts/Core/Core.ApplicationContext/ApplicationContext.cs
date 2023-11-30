using Core.DI;
using Core.Logging;
using System.Collections;
using System.Collections.Generic;
using Core.Management.Audio;
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
        }

        static void BuildDIContext()
        {
            Debug.Log("Building DI context");
            var container = DependencyService.BuildContainer();
            container.RegisterService<ILoggingService, LoggingService>(DependencyType.Singleton);
            container.RegisterService<IAudioService, AudioManager>(DependencyType.Singleton);
            container.Build();
        }
    }
}
