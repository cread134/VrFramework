using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XrCore.XrPhysics.Hands
{

    public class HandCollider : MonoBehaviour
    {
        public Collider[] colliders;

        public void SetColliderActive(bool active)
        {
            foreach (Collider c in colliders)
            {
                c.gameObject.layer = active ? 10 : 11;
            }
        }
    }
}
