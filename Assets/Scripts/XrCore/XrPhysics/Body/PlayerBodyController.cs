using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XrCore.XrPhysics.Body
{
    public class PlayerBodyController : MonoBehaviour
    {
        [Header("Head Settings")]
        [SerializeField] private Transform headTransform;
        [SerializeField] private float neckLength = 0.4f;


        public void OnCalibrationUpdate(float newheight, float newArmLength)
        {

        }

        private void UpdateHead()
        {

        }
    }
}
