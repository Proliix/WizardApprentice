using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] AudioSource[] effectSources;


    int index = 0;

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

        if (effectSources.Length < 1)
            effectSources = gameObject.transform.GetComponentsInChildren<AudioSource>();
    }

    public void PlayAudio(AudioClip clip)
    {
        index = index + 1 < effectSources.Length ? index += 1 : index = 0;
        effectSources[index].PlayOneShot(clip);
    }

    public void PlayAudio(AudioClip clip, float volume)
    {

        index = index + 1 < effectSources.Length ? index += 1 : index = 0;
        effectSources[index].PlayOneShot(clip, volume);
    }
    public void PlayAudio(AudioClip clip, float volume, float pitch)
    {
        effectSources[index].pitch = pitch;

        index = index + 1 < effectSources.Length ? index += 1 : index = 0;
        effectSources[index].PlayOneShot(clip, volume);
        effectSources[index].pitch = 1;
    }

    public void PlayAudio(AudioClip clip, float volume, float maxPitch = 0.8f, float minPitch = 1.2f)
    {
        effectSources[index].pitch = Random.Range(maxPitch, minPitch);

        index = index + 1 < effectSources.Length ? index += 1 : index = 0;
        effectSources[index].PlayOneShot(clip, volume);
        effectSources[index].pitch = 1;
    }
}

