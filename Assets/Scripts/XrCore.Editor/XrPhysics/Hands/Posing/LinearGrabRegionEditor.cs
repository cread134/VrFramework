using UnityEditor;
using UnityEngine.UIElements;

namespace XrCore.XrPhysics.Hands.Posing
{
    [CustomEditor(typeof(LinearGrabRegion))]
    public class LinearGrabRegionEditor : HandTransformReferenceEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            var baseRoot = GetRoot();
            root.Add(new Label("Linear Grab"));
            root.Add(baseRoot);
            return root;
        }
    }
}