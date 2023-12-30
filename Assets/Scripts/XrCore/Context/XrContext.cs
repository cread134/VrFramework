using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XrCore.Interaction.Control;
using XrCore.XrPhysics.Hands;

namespace XrCore.Context
{
    public class XrContext : MonoBehaviour
    {
        [SerializeField] private XrHand leftHand;
        [SerializeField] private XrHand rightHand;
        [SerializeField] private HandController rightHandController;
        [SerializeField] private HandController leftHandController;

        public XrHand GetHand(HandSide side)
        {
            return side == HandSide.Left ? leftHand : rightHand;
        }

        public HandController GetController(HandSide side)
        {
            return side == HandSide.Left ? leftHandController : rightHandController;
        }
    }
}
