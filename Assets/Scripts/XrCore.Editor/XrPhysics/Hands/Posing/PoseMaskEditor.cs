using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace XrCore.XrPhysics.Hands.Posing
{
    [CustomEditor(typeof(PoseMask))]
    public class PoseMaskEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            var targetObj = (PoseMask)target;

            root.Add(new PropertyField(serializedObject.FindProperty("registeredMasks")));
            return root;
        }
    }
}