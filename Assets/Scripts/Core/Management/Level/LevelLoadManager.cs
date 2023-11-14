using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Management.Level
{
    public class LevelLoadManager : MonoBehaviour
    {
        public bool loadOnStart = false;
        private Coroutine _loadCoroutine;
        public event EventHandler<SceneChangeEvent> OnSceneChange;

        //summary
        public void LoadScene(GameScene scene)
        {
            Debug.Log("start scene change");
            if (_loadCoroutine != null)
            {
                StopCoroutine(_loadCoroutine);
            }
            _loadCoroutine = StartCoroutine(PerformSceneSwitch(scene));
        }

        private IEnumerator PerformSceneSwitch(GameScene scene)
        {
            //unload prior scenes
            int sceneCount = SceneManager.sceneCount;
            if (sceneCount > 1)
            {
                Debug.Log("start unloading scenes");
                for (int i = 0; i < sceneCount; i++)
                {
                    int targetSceneIndex = SceneManager.GetSceneAt(i).buildIndex;
                    if (targetSceneIndex != 0)
                    {
                        yield return StartCoroutine(UnloadScene(targetSceneIndex));
                    }
                }
            }
            Debug.Log($"start loading new scene {scene.ToString()}");
            yield return StartCoroutine(LoadAysncScene(scene.BuildIndex));
            Debug.Log("finished scene loading");

            OnSceneChange?.Invoke(this, new SceneChangeEvent(scene));
        }

        private IEnumerator UnloadScene(int buildIndex)
        {
            Debug.Log($"start unloading scene of build index {buildIndex}");
            AsyncOperation op = SceneManager.UnloadSceneAsync(buildIndex);
            while (op != null && !op.isDone)
            {
                float progress = Mathf.Clamp01(op.progress / .9f);

                yield return null;
            }
        }

        private IEnumerator LoadAysncScene(int buildIndex)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
            while (op != null && !op.isDone)
            {
                Debug.Log("not done");
                float progress = Mathf.Clamp01(op.progress / .9f);

                yield return null;
            }
            Debug.Log($"loaded scene of build index {buildIndex}");
        }
    }
}