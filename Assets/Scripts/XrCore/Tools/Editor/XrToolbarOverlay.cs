using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEditor;
using UnityEngine;

namespace XrCore.Tools.Editor
{
    [Overlay(typeof(SceneView), "XrUtilities")]
    [Icon("Assets/unity.png")]
    public class XrToolbarOverlay : ToolbarOverlay
    {
        XrToolbarOverlay() : base(
            XrHandToolbar.id
            )
        {

        }
    }
}
