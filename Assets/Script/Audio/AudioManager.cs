using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    private bool isMuted = false;
    private float previousVolume = 1f;

    void Awake()
    {
        // Initialize all sounds
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        if (isMuted) // Check if audio is muted
            return;

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }

        // Play the sound
        s.source.Play();
    }

    public void MuteAudio()
    {
        isMuted = !isMuted; // Toggle the mute flag

        // Set the volume of all audio sources based on the mute flag
        foreach (Sound s in sounds)
        {
            s.source.volume = isMuted ? 0f : s.volume;
        }
    }

    public void ReplayAudio(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }

        // Replay the sound
        s.source.Play();
    }

    public void UnmuteAudio()
    {
        if (!isMuted)
            return;

        isMuted = false; // Set the unmute flag

        // Restore the volume to previous value
        foreach (Sound s in sounds)
        {
            s.source.volume = previousVolume;
        }

    }
}
