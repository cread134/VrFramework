using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XrCore.XrPhysics.Body
{
    public class XrHead : MonoBehaviour
    {
        [SerializeField] private Transform trackedTransform;
        [SerializeReference] private XrObjectPhysicsConfig config;

        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
