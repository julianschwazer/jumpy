using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // -- Variables for Audio Manager
    public Sound[] sounds;
    public static AudioManager instance;
    
    void Awake()
    {
        // -- Allow the Music to play across Scenes
        DontDestroyOnLoad(gameObject);
        
        // -- Make sure that there is always only one AudioManager across Scenes
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // -- Settings for AudioManager Sounds in Editor
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            
            // -- Options available in Editor
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    // -- Function for playing the right sound file
    public void Play(string name)
    {
        // -- Find the right song in the Array of sounds
        Sound s = Array.Find(sounds, sound => sound.name == name);
        
        // -- Check if AudioFile Name is spelled correctly - Warning
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found! :)");
            return;
        }
        
        s.source.Play();
        
    }

    public void Stop(string name)
    {
        // -- Find the right song in the Array of sounds
        Sound s = Array.Find(sounds, sound => sound.name == name);
        
        s.source.Stop();
    }
    

}