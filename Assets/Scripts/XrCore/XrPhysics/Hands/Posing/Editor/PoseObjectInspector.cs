using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Unity.VisualScripting;

namespace XrCore.XrPhysics.Hands.Posing.Editor
{
    [CustomEditor(typeof(PoseObject))]
    public class PoseObjectInspector : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            // Create a new VisualElement to be the root of our inspector UI
            VisualElement inspectorWindow = new VisualElement();

            PoseObject targetObj = (PoseObject)target;

            // Add a simple label

            if (targetObj.boneNames != null)
            {
                inspectorWindow.Add(new Label("Values are of " + targetObj.boneNames.Length));
                for (int i = 0; i < targetObj.boneNames.Length; i++)
                {
                    inspectorWindow.Add(new Label("Value-> " + targetObj.boneNames[i] + ": " + targetObj.boneValues[i]));
                }
            }
            else
            {
                inspectorWindow.Add(new Label("NullValues!"));
            }

            inspectorWindow.Add(new IMGUIContainer(() => { OnInspectorGUI(); }));

            return inspectorWindow;
        }

        GameObject gameObject;
        UnityEditor.Editor gameObjectEditor;
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            gameObject = (GameObject)EditorGUILayout.ObjectField(gameObject, typeof(GameObject), true);

            if (EditorGUI.EndChangeCheck())
            {
                if (gameObjectEditor != null) DestroyImmediate(gameObjectEditor);
            }

            GUIStyle bgColor = new GUIStyle();
            bgColor.normal.background = EditorGUIUtility.whiteTexture;

            if (gameObject != null)
            {
                if (gameObjectEditor == null)
                    gameObjectEditor = UnityEditor.Editor.CreateEditor(gameObject);
                gameObjectEditor.OnPreviewGUI(GUILayoutUtility.GetRect(256, 256), bgColor);
            }
        }
    }
}
