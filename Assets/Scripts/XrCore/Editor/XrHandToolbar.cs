using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

[EditorToolbarElement(id, typeof(SceneView))]
public class XrHandToolbar : EditorToolbarButton
{
    public const string id = "XrUtilities/HandButton";

    public XrHandToolbar ()
    {
        text = "GetHand";
        clicked += OnClick;
    }

    void OnClick()
    {
        Debug.Log("thing");
    }
}
