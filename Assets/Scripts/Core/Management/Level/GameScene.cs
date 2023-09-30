using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class GameScene : ScriptableObject
{
    public enum SceneType { menu, gameplay, admininstrive }
    public string SceneName;
    public SceneType sceneType;
    public int BuildIndex;

    public override string ToString()
    {
        return $"Scene: {SceneName}: Index: {BuildIndex} Type: {sceneType}";
    }
}
