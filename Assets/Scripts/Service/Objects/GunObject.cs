using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XrCore.XrPhysics.Interaction;
using XrCore.XrPhysics.World;

namespace Service.Objects
{
    public class GunObject : MonoBehaviour, IGrabObjectEvents
    {
        private GrabbableObject GrabOject 
        { get
            {
                return grabbableObject ??= GetComponent<GrabbableObject>();
            } 
        }
        [SerializeField] private GrabbableObject grabbableObject;
        public GunData gunData;
        [SerializeField] private GrabPoint triggerPoint;

        private void Awake()
        {
            grabbableObject.SubscribeEvents(this);
        }

        public void OnGripChange(float oldValue, float newValue, GrabPoint source)
        {

        }

        public void OnGripFinished(GrabPoint source)
        {

        }

        public void OnGripStarted(GrabPoint source)
        {

        }

        public void OnMainDown(GrabPoint source)
        {
            if (source != triggerPoint) return;
            Debug.Log("Ejection action");
        }

        public void OnMainUp(GrabPoint source)
        {

        }

        public void OnTriggerChange(float oldValue, float newValue, GrabPoint source)
        {

        }

        public void OnTriggerDown(GrabPoint source)
        {
            if (source != triggerPoint) return;
            Debug.Log("trigger pressed");
        }

        public void OnTriggerUp(GrabPoint source)
        {

        }
    }
}