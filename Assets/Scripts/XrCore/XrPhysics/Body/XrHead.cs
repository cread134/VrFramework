using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using XrCore.XrPhysics.World;

namespace XrCore.XrPhysics.Body
{
    public class XrHead : MonoBehaviour
    {
        [SerializeField] private Transform trackedTransform;
        [SerializeReference] private XrObjectPhysicsConfig physicsSettings;

        private Rigidbody _rigidbody;
        PhysicsMover _physicsMover;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _physicsMover = new PhysicsMover(physicsSettings, _rigidbody);
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
        void MatchTarget()
        {
            _physicsMover.PhysicsMatchPosition(trackedTransform.position);
        }
    }
}
