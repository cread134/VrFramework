using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

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
        private Queue<SoundInstance> _localSoundInstances;
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

            _localSoundInstances = new Queue<SoundInstance>();
            for (int i = 0; i < soundInstancesToCreate; i++)
            {
                _localSoundInstances.Enqueue(CreateSoundInstanceEmpty("loca_" + i.ToString(), true));
            }
        }

#endregion

        private SoundInstance CreateSoundInstanceEmpty(string tag, bool isLocal = false)
        {
            var objInstance = new GameObject("soundInstance_" + tag);
            objInstance.transform.SetParent(transform, false);
            var audioSource = objInstance.AddComponent<AudioSource>();
            objInstance.AddComponent<SoundInstance>();

            audioSource.spatialBlend = isLocal ? 0 : 1;

            objInstance.SetActive(false);
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
                return PlaySound(useSound, position, volumeMultipler, isLocal);
            }
            else
            {
                throw new Exception($"Sound with key {soundKey} does not exist!");
            }
        }

        public SoundInstance PlaySound(GameSound targetSound, float volumeMultipler, bool isLocal = false)
            => PlaySound(targetSound, Vector3.zero, volumeMultipler, isLocal);

        public SoundInstance PlaySound(GameSound targetSound, Vector3 position, float volumeMultipler, bool isLocal = false)
        {
            var useQueue = isLocal ? _localSoundInstances : _createdSoundInstances;
            SoundInstance targetInstance = useQueue.Dequeue();
            AudioClip soundClip = targetSound.playableClips[UnityEngine.Random.Range(0, targetSound.playableClips.Length)];
            targetInstance.PlayInstanceSound(position, volumeMultipler, targetSound.soundRange, soundClip, targetSound.soundMixerGroup);
            useQueue.Enqueue(targetInstance);
            return targetInstance;
        }
    }
}
