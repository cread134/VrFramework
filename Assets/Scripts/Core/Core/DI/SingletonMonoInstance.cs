using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.DI
{
    public class SingletonMonoInstance : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}