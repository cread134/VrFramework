using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace XrCore.Tools.Editor
{
    [EditorToolbarElement(id, typeof(SceneView))]
    public class XrHandToolbar : EditorToolbarButton
    {
        public const string id = "XrUtilities/HandButton/Right";

        public XrHandToolbar()
        {
            text = "GetHand";
            clicked += OnClick;
        }

        void OnClick()
        {
            GameObject targetHand = GameObject.Find("RightHandTracked");
            if (targetHand != null)
            {
                Selection.activeGameObject = targetHand;
                SceneView.lastActiveSceneView.FrameSelected();
            }
        }
    }
}
