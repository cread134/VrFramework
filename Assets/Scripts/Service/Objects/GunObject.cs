using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XrCore.XrPhysics.World;

namespace Service.Objects
{
    public class GunObject : MonoBehaviour, IGrabObjectEvents
    {
        public GrabbableObject grabbableObject;
        public GunData gunData;

        private void Awake()
        {
            grabbableObject.SubscribeEvents(this);
        }

        void IGrabObjectEvents.OnGripChange(float oldValue, float newValue)
        {

        }

        void IGrabObjectEvents.OnGripFinished()
        {

        }

        void IGrabObjectEvents.OnGripStarted()
        {

        }

        void IGrabObjectEvents.OnMainDown()
        {

        }

        void IGrabObjectEvents.OnMainUp()
        {

        }

        void IGrabObjectEvents.OnTriggerChange(float oldValue, float newValue)
        {

        }

        void IGrabObjectEvents.OnTriggerDown()
        {

        }

        void IGrabObjectEvents.OnTriggerUp()
        {

        }
    }
}