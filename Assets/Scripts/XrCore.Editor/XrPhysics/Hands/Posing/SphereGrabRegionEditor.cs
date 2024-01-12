using UnityEditor;
using UnityEngine.UIElements;
using Core.Extensions;

namespace XrCore.XrPhysics.Hands.Posing
{
    [CustomEditor(typeof(SphereGrabRegion))]
    public class SphereGrabRegionEditor : HandTransformReferenceEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            var baseRoot = GetRoot();
            root.AddHeader("Sphere Grab Region");
            root.Add(baseRoot);
            return root;
        }
    }
}