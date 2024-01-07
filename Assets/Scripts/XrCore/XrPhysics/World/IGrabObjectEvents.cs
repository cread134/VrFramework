using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XrCore.XrPhysics.Interaction;

namespace XrCore.XrPhysics.World
{
    public interface IGrabObjectEvents
    {
        public void OnGripStarted(GrabPoint source);
        public void OnGripFinished(GrabPoint source);
        public void OnGripChange(float oldValue, float newValue, GrabPoint source);
        public void OnTriggerChange(float oldValue, float newValue, GrabPoint source);
        public void OnTriggerDown(GrabPoint source);
        public void OnTriggerUp(GrabPoint source);
        public void OnMainDown(GrabPoint source);
        public void OnMainUp(GrabPoint source);
    }
}