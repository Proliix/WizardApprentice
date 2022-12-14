using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMusicScript : MonoBehaviour
{
    public AudioClip intro;
    public AudioClip loop;

    bool loopStarted = false;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = intro;
        audioSource.Play();
    }

    private void Update()
    {
        if (audioSource.time >= intro.length && !loopStarted)
        {
            loopStarted = true;
            StartLoop();
        }
    }

    void StartLoop()
    {
        audioSource.clip = loop;
        audioSource.loop = true;
        audioSource.Play();
    }

}
