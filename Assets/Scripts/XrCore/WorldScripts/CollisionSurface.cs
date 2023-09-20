using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CollisionSurface : ScriptableObject 
{
    [SerializeField] private string collisionName;
    public override string ToString()
    {
        return collisionName;
    }

    [Range(0f, 1f)]public float hardness;   
}


//Interaction amtric value
[System.Serializable]
public class CollisionInteraction
{
    public CollisionSurface collisionA;

    public CollisionSurface collisionB;

    [SerializeField] private string particleKey;
    public string ParticleKey() { return particleKey; }

    [SerializeField] private AudioClip[] clips;

    public AudioClip CollisionSound()
    {
        return clips[Random.Range(0, clips.Length)];
    }
}
