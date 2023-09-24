using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEditor.SceneManagement;

public class RootSceneLoader 
{
    [MenuItem("Tools/LoadRootScene")]
    public static void LoadRootScene()
    {
        EditorSceneManager.SaveOpenScenes();
        var Scenes = EditorBuildSettings.scenes;
        EditorSceneManager.OpenScene(Scenes[0].path);    
    }
}
