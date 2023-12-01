using UnityEditor;
using UnityEngine.UIElements;
using XrCore.XrPhysics.Hands.Posing;


namespace XrCore.XrPhysics.Hands.Posing.Editor
{
    [CustomEditor(typeof(DirectGrabRegion))]
    public class DirectGrabRegionEditor : HandTransformReferenceEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            var baseRoot = GetRoot();
            root.Add(new Label("Direct Grab"));
            root.Add(baseRoot);
            return root;
        }
    }
}