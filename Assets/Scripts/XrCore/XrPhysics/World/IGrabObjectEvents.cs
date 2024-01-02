using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XrCore.XrPhysics.World
{
    public interface IGrabObjectEvents
    {
        public void OnGripStarted();
        public void OnGripFinished();
        public void OnGripChange(float oldValue, float newValue);
        public void OnTriggerChange(float oldValue, float newValue);
        public void OnTriggerDown();
        public void OnTriggerUp();
        public void OnMainDown();
        public void OnMainUp();
    }
}