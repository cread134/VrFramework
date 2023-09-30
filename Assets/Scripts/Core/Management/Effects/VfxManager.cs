using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Rendering;

public class VfxManager : MonoBehaviour
{
    public int instancesPerType = 10;
    // Label strings to load
    public string key;

    // Operation handle used to load and release assets
    AsyncOperationHandle<IList<VfxObject>> loadHandle;

    private Dictionary<string, Queue<GameObject>> vfxDictionary;



    // Load Addressables by Label
    public IEnumerator Start()
    {
        vfxDictionary = new Dictionary<string, Queue<GameObject>>();
        loadHandle = Addressables.LoadAssetsAsync<VfxObject>(
            key,
            addressable => {
                //Gets called for every loaded asset
                Queue<GameObject> queue = new Queue<GameObject>();
                for (int i = 0; i < instancesPerType; i++)
                {
                    GameObject createdInstance = Instantiate(addressable.prefab, transform);
                    queue.Enqueue(createdInstance);
                    createdInstance.SetActive(false);
                }
                vfxDictionary.Add(addressable.key, queue);
            },
            false); // Whether to fail and release if any asset fails to load

        yield return loadHandle;
    }

    public void SpawnParticleInstance(string key, Vector3 position, Quaternion rotation)
    {
        if (vfxDictionary.ContainsKey(key))
        {
            GameObject particleInstance = vfxDictionary[key].Dequeue();
            particleInstance.SetActive(true);
            particleInstance.transform.position = position;
            particleInstance.transform.rotation = rotation;
            ParticleSystem particleSystem = particleInstance.GetComponent<ParticleSystem>();
            particleSystem.Play();
            var system = particleSystem.main;
            system.loop = false;
            system.stopAction = ParticleSystemStopAction.Disable;

            vfxDictionary[key].Enqueue(particleInstance);
        }
    }

    private void OnDestroy()
    {
       // Addressables.Release(loadHandle);
    }
}
