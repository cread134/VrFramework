using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class ManagerAssurance
{
    [MenuItem("Tools/CreateGameManager")]
    public static void AssureGameManager()
    {
        var objs = GameObject.FindObjectOfType<GameManager>();
        if(objs == null )
        {
            var managerPre = Resources.Load("GameManager") as GameObject;
            GameObject.Instantiate(managerPre);
            EditorSceneManager.SaveOpenScenes();
        }
    }
}
  