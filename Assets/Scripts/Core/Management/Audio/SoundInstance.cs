using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundInstance : MonoBehaviour
{
    private AudioSource _audioSource;
    public AudioSource audioSource {  get { return _audioSource; } }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayInstanceSound(Vector3 pos, float volumeMultipler, float range, AudioClip desiredClip, AudioMixerGroup soundGroup)
    {
        transform.position = pos;
        _audioSource.minDistance = range;
        _audioSource.outputAudioMixerGroup = soundGroup;
        _audioSource.PlayOneShot(desiredClip, volumeMultipler);
    }
}
