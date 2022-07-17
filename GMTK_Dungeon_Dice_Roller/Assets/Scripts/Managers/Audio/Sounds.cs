using System;
using UnityEngine;

[Serializable]
public class Sounds
{
    public string name;
    public AudioClip clip;

    [Range(0f,1f)]
    public float volume;
    [Range(0.1f,3f)]
    public float pitch = 1f;

    [HideInInspector]
    public AudioSource source;
}
