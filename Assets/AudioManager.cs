using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;

    public static AudioManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
            s.source.pitch = s.pitch;
            s.source.playOnAwake = s.playOnAwake;
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if(s == null)
        {
            Debug.Log("no s'ha pogut reproduir " + name);
            return;
        }
        s.source.Play();
    }

    public void StopSFX(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.Log("no s'ha pogut aturar " + name);
            return;
        }
        s.source.Stop();
    }
}
