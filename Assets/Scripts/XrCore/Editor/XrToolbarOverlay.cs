using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEditor;
using UnityEngine;

[Overlay(typeof(SceneView), "XrUtilities")]
[Icon("Assets/unity.png")]
public class XrToolbarOverlay : ToolbarOverlay
{
    XrToolbarOverlay() : base(XrHandToolbar.id) { }
}
