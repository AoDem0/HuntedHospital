using System;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string soundName;
    public AudioClip clip;
    [Range(0f, 1f)] 
    public float volume;
    [Range(0f, 1f)] 
    public float pitch;

    [HideInInspector] public AudioSource source;
}

public enum SoundType
{
    Music,
    SoundEffect
}
public class soundManager : MonoBehaviour
{
    public Sound[] sounds;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
        }
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.soundName == name);
        s.source.Play();
    }

    public void ChangeVolume(SoundType type, float num)
    {
        if (type == SoundType.Music)
        {
            
        }
        else
        {
            
        }
    }
}
