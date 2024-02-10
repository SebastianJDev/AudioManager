using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public List<Sound> sounds;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
        }
    }
    public void AddSound(Sound sound)
    {
        sounds.Add(sound);
    }
    public void Play(string name)
    {
        Sound sound = sounds.Find(s => s.name == name);
        if (sound != null)
        {
            sound.source.Play();
        }
        else
        {
            Debug.LogWarning("Sound with name " + name + " not found!");
        }
    }

    public void Stop(string name)
    {
        Sound sound = sounds.Find(s => s.name == name);
        if (sound != null)
        {
            sound.source.Stop();
        }
        else
        {
            Debug.LogWarning("Sound with name " + name + " not found!");
        }
    }

    public void SetVolume(string name, float volume)
    {
        Sound sound = sounds.Find(s => s.name == name);
        if (sound != null)
        {
            sound.source.volume = volume;
        }
        else
        {
            Debug.LogWarning("Sound with name " + name + " not found!");
        }
    }
    public void ChangeName(Sound sound,string newName)
    {
        sound.name = newName;
    }
    public void RemoveSound(Sound sound)
    {
        sounds.Remove(sound);
    }
}
