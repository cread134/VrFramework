using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XrCore.Configuration;

namespace XrCore.XrPhysics.Body
{
    public class XrBody : MonoBehaviour, IXrCallibration
    {
        [SerializeField] private float targetHeight = 1.8f;

        [Header("MainComponents")]
        [SerializeField] private Transform hipTransform;
        [SerializeField] private Transform headTransform;

        public void CallibratePlayer(float targetHeight, float armLength)
        {

        }

        private void OnDrawGizmos()
        {
            //draw body
            if (hipTransform != null && headTransform != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(headTransform.position, 0.15f);
                Gizmos.DrawWireSphere(hipTransform.position, 0.3f);
            }
        }
    }
}
