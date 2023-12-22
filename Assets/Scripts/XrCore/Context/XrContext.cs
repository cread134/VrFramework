using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XrCore.XrPhysics.Hands;

namespace XrCore.Context
{
    public class XrContext : MonoBehaviour
    {
        [SerializeField] private XrHand leftHand;
        [SerializeField] private XrHand rightHand;

        public XrHand GetHand(HandSide side)
        {
            return side == HandSide.Left ? leftHand : rightHand;
        }
    }
}
