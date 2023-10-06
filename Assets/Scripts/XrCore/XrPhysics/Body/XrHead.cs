using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace XrCore.XrPhysics.Body
{
    public class XrHead : MonoBehaviour
    {
        [SerializeField] private Transform trackedTransform;
        [SerializeReference] private XrObjectPhysicsConfig physicsSettings;

        private Rigidbody _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            _rigidbody.MoveRotation(trackedTransform.rotation);
        }

        private void FixedUpdate()
        {
            MatchTarget();
        }
        private Vector3 positionError;
        private Vector3 lastPositionError;
        private Vector3 positionStoredIntegration;
        void MatchTarget()
        {
            positionError = trackedTransform.position - _rigidbody.position;

            Vector3 positionProportion = positionError * physicsSettings.positionIntegrationCompenstation;

            Vector3 derivativeGain = (positionError - lastPositionError) / Time.fixedDeltaTime;
            Vector3 positionDerivative = derivativeGain * physicsSettings.positionSmoothing;

            lastPositionError = positionError;

            positionStoredIntegration += (positionError * Time.fixedDeltaTime);

            Vector3 force = positionProportion + positionStoredIntegration + positionDerivative;
            // Debug.Log("pid force " + force);
            _rigidbody.AddForce(force);
        }
    }
}
