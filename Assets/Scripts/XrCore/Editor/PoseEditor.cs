using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;
public class PoseEditor : EditorWindow
{
    private string POSEFILEPATH = "Assets/XrCore/ScriptableObjects/HandPoses";

     TextField m_createPoseName;
    
    [MenuItem("Tools/PoseEditor")]
    public static void DisplayEditor()
    {
        // This method is called when the user selects the menu item in the Editor
        EditorWindow wnd = GetWindow<PoseEditor>();
        wnd.titleContent = new GUIContent("Hand Pose Editor");
    }
    public void CreateGUI()
    {
        rootVisualElement.Clear();
        rootVisualElement.Add(new Label("Pose Editor"));

        GameObject targetObject = Selection.activeGameObject;
        if (!ValidateSelection(targetObject))
        {        
            return;
        }
        m_createPoseName = new TextField
        {
            label = "Pose Name",
            multiline = false,
            isDelayed = true
        };
        
        rootVisualElement.Add(m_createPoseName);

        rootVisualElement.Add(new Label("Selected posable object -> " + targetObject.name));
        Button bakeButton = new Button();
        bakeButton.text = "Bake Pose";
        bakeButton.clicked += OnBakeClick;
        rootVisualElement.Add(bakeButton); 
    }

    public void OnBakeClick()
    {
        if(Selection.activeGameObject.TryGetComponent<PosableHandObject>(out PosableHandObject handObject)){
            CreateNewPose(handObject.BakeHandPose());
        }
    }

    private void OnSelectionChange()
    {
        CreateGUI();
    }

    private void CreateNewPose(HandPose pose)
    {

        string fileName = "HandPose_" + m_createPoseName.value;
        PoseObject scriptableInstance = ScriptableObject.CreateInstance<PoseObject>();
        AssetDatabase.CreateAsset(scriptableInstance, $"{POSEFILEPATH}/{fileName}.asset");
        var so = new SerializedObject(scriptableInstance);
        so.FindProperty("_poseName").stringValue = m_createPoseName.value;
        so.FindProperty("boneNames").arraySize = pose.poseValues.Count;
        so.FindProperty("boneValues").arraySize = pose.poseValues.Count;
        for (int i = 0; i < pose.poseValues.Count; i++)
        {
            so.FindProperty("boneNames").GetArrayElementAtIndex(i).stringValue = pose.poseValues.Keys.ToArray()[i];
            so.FindProperty("boneValues").GetArrayElementAtIndex(i).quaternionValue = pose.poseValues.Values.ToArray()[i];
        }
        so.ApplyModifiedProperties();
        EditorUtility.SetDirty( scriptableInstance );
        //i hopes save worked
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = scriptableInstance;
        AssetDatabase.SaveAssets();
    }

    private bool ValidateSelection(GameObject target)
    {
        if(target == null) return false;
        if (!target.TryGetComponent<PosableHandObject>(out PosableHandObject posableHandObject)) return false;
        return true;
    }
}
