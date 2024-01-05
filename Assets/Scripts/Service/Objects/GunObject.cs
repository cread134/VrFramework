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

        public void OnGripChange(float oldValue, float newValue)
        {

        }

        public void OnGripFinished()
        {

        }

        public void OnGripStarted()
        {

        }

        public void OnMainDown()
        {

        }

        public void OnMainUp()
        {

        }

        public void OnTriggerChange(float oldValue, float newValue)
        {

        }

        public void OnTriggerDown()
        {

        }

        public void OnTriggerUp()
        {

        }
    }
}