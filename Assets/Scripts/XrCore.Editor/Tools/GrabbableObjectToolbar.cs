using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace XrCore.Tools
{
    [Overlay(typeof(SceneView), "ObjectControls", true)]
    public class GrabbableObjectToolbar : Overlay
    {
        public override VisualElement CreatePanelContent()
        {
            VisualElement root = new VisualElement { name = "ObjectControls" };
            return root;
        }
    }
}
