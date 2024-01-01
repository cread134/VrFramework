using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEditor.Overlays;
using UnityEngine.UIElements;
using Core.Extensions;
using XrCore.XrPhysics.Hands;
using XrCore.Context;
using static UnityEngine.GraphicsBuffer;
using System.Linq;

namespace XrCore.Tools.Editor
{
    [Overlay(typeof(SceneView), "Xr Controls", true)]
    public class XrHandToolbar : Overlay
    {
        public override VisualElement CreatePanelContent()
        {
            var root = new VisualElement() { name = "Xr Toolbar" };
            root.AddHeader("XR TOOLS");

            root.AddButton("RightHand", AccessRightHand);
            root.AddButton("LeftHand", AccessLeftHand);

            return root;

        }

        void AccessRightHand() => AccessHandCore(HandSide.Right);
        void AccessLeftHand() => AccessHandCore(HandSide.Left);

        void AccessHandCore(HandSide handSide)
        {
            var context = GameObject.FindFirstObjectByType<XrContext>();
            var hand = context?.GetController(handSide);
            if (hand != null)
            {
                Selection.activeTransform = hand.transform;
                SceneView.lastActiveSceneView.FrameSelected();
            }

            CreatePanelContent();
        }
    }
}
