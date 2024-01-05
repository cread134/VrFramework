using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;

namespace ScriptingResources
{
    public static class OverlayExtensions
    {
        public static void Redraw(this Overlay overlay)
        {
            //this fucking sucks, but its the only way to repaint :(
            overlay.collapsed = true;
            overlay.collapsed = false;
        }
    }
}
