using System;
using UnityEngine;

namespace Core.Management.Audio
{
    public interface IAudioService
    {
        public SoundInstance PlaySound(Guid soundKey, Vector3 position, float volumeMultipler, bool isLocal);
        public SoundInstance PlaySound(GameSound targetSound, float volumeMultipler, bool isLocal = false);
        public SoundInstance PlaySound(GameSound targetSound, Vector3 position, float volumeMultipler, bool isLocal = false);
    }
}