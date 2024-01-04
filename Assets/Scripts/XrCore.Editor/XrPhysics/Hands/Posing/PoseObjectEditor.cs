using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace XrCore.XrPhysics.Hands.Posing
{
    [CustomEditor(typeof(PoseObject))]
    public class PoseObjectEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            // Create a new VisualElement to be the root of our inspector UI
            VisualElement inspectorWindow = new VisualElement();

            PoseObject targetObj = (PoseObject)target;
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

        private void OnEnable()
        {
            gameObject = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/XrCore/LeftHandPossable.prefab", typeof(GameObject)) as GameObject;
        }

        GameObject gameObject;
        UnityEditor.Editor gameObjectEditor;
        public override void OnInspectorGUI()
        {
            PoseObject targetObj = (PoseObject)target;

            GUIStyle bgColor = new GUIStyle();
            bgColor.normal.background = EditorGUIUtility.whiteTexture;

            if (gameObject != null)
            {
                var possable = gameObject.GetComponent<PosableHandObject>();
                possable.UpdateHandPose(targetObj.HandPose);

                if (gameObjectEditor == null)
                    gameObjectEditor = UnityEditor.Editor.CreateEditor(gameObject);
                gameObjectEditor.OnPreviewGUI(GUILayoutUtility.GetRect(256, 256), bgColor);
            }
        }
    }
}
