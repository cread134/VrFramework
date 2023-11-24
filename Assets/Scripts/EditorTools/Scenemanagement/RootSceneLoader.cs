using UnityEditor;
using UnityEditor.SceneManagement;

namespace EditorTools.SceneManagement
{
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
}