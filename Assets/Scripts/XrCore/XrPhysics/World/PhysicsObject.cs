using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XrCore.XrPhysics.XrCollision;

namespace XrCore.XrPhysics.World
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(WorldSurface))]
    public class PhysicsObject : MonoBehaviour
    {
        public void OnCollisionEnter(Collision collision)
        {

        }
    }
}