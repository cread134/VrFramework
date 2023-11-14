using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Core.Management.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private string key;
        [SerializeField] private GameObject soundInstancePrefab;
        [SerializeField] private int soundInstancesToCreate = 30;
        [SerializeField] private AudioSource localSource;

        private Queue<SoundInstance> _createdSoundInstances;
        private Dictionary<string, GameSound> _soundsDictionary;

        // Operation handle used to load and release assets
        AsyncOperationHandle<IList<GameSound>> loadHandle;
        private bool _loaded;
        private void Start()
        {
            InitialiseAudioContainers();
            StartCoroutine(InitialiseAudio());
        }
        IEnumerator InitialiseAudio()
        {
            _loaded = false;
            _soundsDictionary = new Dictionary<string, GameSound>();
            loadHandle = Addressables.LoadAssetsAsync<GameSound>(
                key,
                addressable =>
                {
                    //Gets called for every loaded asset
                    _soundsDictionary.Add(addressable.soundKey, addressable);
                },
                false); // Whether to fail and release if any asset fails to load

            yield return loadHandle;
            _loaded = true;
        }
        private void InitialiseAudioContainers()
        {
            _createdSoundInstances = new Queue<SoundInstance>();
            for (int i = 0; i < soundInstancesToCreate; i++)
            {
                GameObject instance = Instantiate(soundInstancePrefab, transform);
                _createdSoundInstances.Enqueue(instance.GetComponent<SoundInstance>());
            }
        }

        public void PlaySound(string soundName, Vector3 position, float volumeMultipler, bool isLocal)
        {
            if (!_loaded)
            {
                Debug.Log("Sounds have not finished loading");
                return;
            }
            if (_soundsDictionary.ContainsKey(soundName))
            {
                GameSound useSound = _soundsDictionary[soundName];
                AudioClip soundClip = useSound.playableClips[Random.Range(0, useSound.playableClips.Length)];
                if (isLocal)
                {
                    localSource.outputAudioMixerGroup = useSound.soundMixerGroup;
                    localSource.PlayOneShot(soundClip, volumeMultipler);
                }
                else
                {
                    SoundInstance targetInstance = _createdSoundInstances.Dequeue();
                    targetInstance.PlayInstanceSound(position, volumeMultipler, useSound.soundRange, soundClip, useSound.soundMixerGroup);
                    _createdSoundInstances.Enqueue(targetInstance);
                }
            }
            else
            {
                Debug.LogError($"Sound of name {soundName} does not exist!");
            }
        }
    }
}
