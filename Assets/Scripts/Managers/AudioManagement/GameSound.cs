using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu]
public class GameSound : ScriptableObject
{
    public string soundKey;
    public AudioMixerGroup soundMixerGroup;
    public AudioClip[] playableClips;
    public float soundRange = 1f;
    public float volume = 1f;
}
