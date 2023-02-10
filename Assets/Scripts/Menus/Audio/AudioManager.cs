using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;

    private void Awake()
    {
        // singleton
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.playOnAwake = false;

            if (s.clip.name.Contains("V_Main_Theme") || s.clip.name.Contains("V_Combat_Theme"))
            {
                s.source.loop = true;
            } 
        }
    }

    //TODO v2: Accept a SoundType enum instead of a string
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Stop();
    }

    public void StopAll()
    {
        foreach(Sound sound in sounds){
            if (sound == null)
                return;
            sound.source.Stop();
        }
    }

    public void StopAllAndPlay(string name){
        FindObjectOfType<AudioManager>().StopAll();
        FindObjectOfType<AudioManager>().Play(name);
    }

    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Pause();
    }

    public bool IsSourcePlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return false;
        return s.source.isPlaying;
    }

    public void ChangeVolume(string name, float volume)
    {
        foreach (Sound sound in sounds)
        {
            if(sound.name.Contains(name))
                sound.source.volume = volume;
        }
    }
}
