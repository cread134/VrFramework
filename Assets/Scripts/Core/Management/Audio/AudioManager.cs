using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Core.Management.Audio
{
    public class AudioManager : MonoBehaviour
    {
        #region singleton

        private static AudioManager _instance;
        public static AudioManager Instance 
        { get 
            {
                if (_instance == null)
                    _instance = new AudioManager();
                return _instance;
            } 
        }

        void InitSingleton()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            _instance = this;
        }

        #endregion

        private const string key = "Sound";

        [SerializeField] private int soundInstancesToCreate = 30;

        private Queue<SoundInstance> _createdSoundInstances;
        private Dictionary<Guid, GameSound> _soundsDictionary;
        private SoundInstance localSource;

        // Operation handle used to load and release assets
        AsyncOperationHandle<IList<GameSound>> loadHandle;
        private bool _loaded;

        private void Awake()
        {
            InitSingleton();
        }

        private void Start()
        {
            InitialiseAudioContainers();
            StartCoroutine(InitialiseAudio());
            localSource = CreateSoundInstanceEmpty("localSource", isLocal: true);
        }
        #region initialization
        IEnumerator InitialiseAudio()
        {
            _loaded = false;
            _soundsDictionary = new Dictionary<Guid, GameSound>();
            loadHandle = Addressables.LoadAssetsAsync<GameSound>(
                key,
                addressable =>
                {
                    //Gets called for every loaded asset]
                    addressable.soundKey = Guid.NewGuid();
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
                _createdSoundInstances.Enqueue(CreateSoundInstanceEmpty(i.ToString()));
            }
        }

#endregion

        private SoundInstance CreateSoundInstanceEmpty(string tag, bool isLocal = false)
        {
            var objInstance = new GameObject("soundInstance_" + tag);
            var audioSource = objInstance.AddComponent<AudioSource>();
            objInstance.AddComponent<SoundInstance>();

            audioSource.spatialBlend = isLocal ? 0 : 1;
           
            return new SoundInstance();
        }

        public SoundInstance PlaySound(Guid soundKey, Vector3 position, float volumeMultipler, bool isLocal)
        {
            if (!_loaded)
            {
                Debug.Log("Sounds have not finished loading");
                return null;
            }
            if (_soundsDictionary.ContainsKey(soundKey))
            {
                GameSound useSound = _soundsDictionary[soundKey];
                AudioClip soundClip = useSound.playableClips[UnityEngine.Random.Range(0, useSound.playableClips.Length)];
                if (isLocal)
                {
                    localSource.audioSource.outputAudioMixerGroup = useSound.soundMixerGroup;
                    localSource.audioSource.PlayOneShot(soundClip, volumeMultipler);
                    return localSource;
                }
                else
                {
                    SoundInstance targetInstance = _createdSoundInstances.Dequeue();
                    targetInstance.PlayInstanceSound(position, volumeMultipler, useSound.soundRange, soundClip, useSound.soundMixerGroup);
                    _createdSoundInstances.Enqueue(targetInstance);
                    return targetInstance;
                }
            }
            else
            {
                throw new Exception($"Sound with key {soundKey} does not exist!");
            }
        }
    }
}
