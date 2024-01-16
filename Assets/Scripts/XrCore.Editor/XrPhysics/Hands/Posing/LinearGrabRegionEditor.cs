using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Core.Extensions;
using UnityEditor.UIElements;
using System.Linq;

namespace XrCore.XrPhysics.Hands.Posing
{
    [CustomEditor(typeof(LinearGrabRegion))]
    public class LinearGrabRegionEditor : HandTransformReferenceEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            var baseRoot = GetRoot();
            root.AddHeader("Linear Grab");
            root.Add(baseRoot);

            root.Add(new Label("Linear settings"));

            return root;
        }

        public override void ValidateObject(HandTransformReference targ)
        {
            var linear = targ as LinearGrabRegion;
            if(linear.start != null && linear.end != null && linear.middle != null)
            {
                return;
            }
            base.ValidateObject(targ);

            var start = new GameObject() { name = $"{linear.name}_start" };
            start.transform.parent = linear.transform;
            start.transform.localPosition = new Vector3(0f, 0f, 0.1f);

            var end = new GameObject() { name = $"{linear.name}_end"};
            end.transform.parent = linear.transform;
            end.transform.localPosition = new Vector3(0f, 0f, -0.1f);

            var middle = new GameObject() { name = $"{linear.name}_middle" };
            middle.transform.parent = linear.transform;
            middle.transform.position = Vector3.Lerp(start.transform.position, end.transform.position, 0.5f);

            linear.start = start.transform;
            linear.end = end.transform;
            linear.middle = middle.transform;

            EditorUtility.SetDirty(start);
            EditorUtility.SetDirty(end);
            EditorUtility.SetDirty(middle);
            EditorUtility.SetDirty(linear);
        }

        public override Vector3 GetPreviewPosition()
        {
            var linear = target as LinearGrabRegion;
            return linear.middle.transform.position;
        }
    }
}