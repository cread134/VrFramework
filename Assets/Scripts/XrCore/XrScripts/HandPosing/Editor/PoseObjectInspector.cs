using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Unity.VisualScripting;

namespace XrCore.Physics.Hands.Posing.Editor
{
    [CustomEditor(typeof(PoseObject))]
    public class PoseObjectInspector : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            // Create a new VisualElement to be the root of our inspector UI
            VisualElement myInspector = new VisualElement();

            PoseObject targetObj = (PoseObject)target;

            // Add a simple label

            if (targetObj.boneNames != null)
            {
                myInspector.Add(new Label("Values are of " + targetObj.boneNames.Length));
                for (int i = 0; i < targetObj.boneNames.Length; i++)
                {
                    myInspector.Add(new Label("Value-> " + targetObj.boneNames[i] + ": " + targetObj.boneValues[i]));
                }
            }
            else
            {
                myInspector.Add(new Label("NullValues!"));
            }
            // Return the finished inspector UI
            return myInspector;
        }
    }
}
