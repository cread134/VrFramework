using Core.Management.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class AudioManagerExtensions
{
    public static SoundInstance Play(this GameSound sound, float volumeMultipler = 1f) 
        => AudioManager.Instance.PlaySound(sound, volumeMultipler, true);

    public static SoundInstance Play(this GameSound sound, Vector3 position, float volumeMultipler = 1f) 
        => AudioManager.Instance.PlaySound(sound, position, volumeMultipler);
}
