using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]  AudioSource musicSource, effectSource;

    //Singleton instance 
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayAudio(AudioClip clip, float volume = 1)
    {
        
        effectSource.PlayOneShot(clip,volume);
    }
    public void PlayAudio(AudioClip clip, float volume = 1, float pitch = 1)
    {
        effectSource.pitch = pitch;

        effectSource.PlayOneShot(clip, volume);
        effectSource.pitch = 1;
    }

    public void PlayAudio(AudioClip clip, float volume = 1, float maxPitch = 0.8f, float minPitch = 1.2f)
    {
        effectSource.pitch = pitch;

        effectSource.PlayOneShot(clip, volume);
        effectSource.pitch = Random.Range(maxPitch, minPitch);
    }
}

