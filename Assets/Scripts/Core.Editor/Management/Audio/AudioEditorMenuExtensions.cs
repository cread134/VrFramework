using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Core.Management.Audio.Editor
{
    public class AudioEditorMenuExtensions
    {
        const string SOUND_FOLDER_PATH = "Assets/ScriptableObjects/Sounds";

        [MenuItem("Assets/Create/GameSound", priority = 1)]
        public static void CreateGameSound()
        {
            var soundName = "newSound";
            var createdInstance = ScriptableObject.CreateInstance<GameSound>();
            var path = System.IO.Path.Combine(SOUND_FOLDER_PATH, soundName) + ".asset";
            var uniqueName = AssetDatabase.GenerateUniqueAssetPath(path);
            AssetDatabase.CreateAsset(createdInstance, uniqueName);

            var clips = GetSelectedClips();
            if (clips.Count > 0)
            {
                createdInstance.playableClips = clips.ToArray();
            }

            EditorUtility.SetDirty(createdInstance);
            AssetDatabase.SaveAssets();
            Selection.activeObject = createdInstance;
            EditorUtility.FocusProjectWindow();
        }

        public static List<AudioClip> GetSelectedClips()
        {
            var selected = Selection.GetFiltered(typeof(AudioClip), SelectionMode.Assets);
            return selected.Cast<AudioClip>().ToList();
        }
    }
}