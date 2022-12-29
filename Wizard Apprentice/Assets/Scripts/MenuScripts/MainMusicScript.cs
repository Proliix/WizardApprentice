using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMusicScript : MonoBehaviour
{
    public AudioClip intro;
    public AudioClip loop;

    bool loopStarted = false;
    AudioSource audioSource;
    Animator anim;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        anim.SetTrigger("FadeIn");
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = intro;
        audioSource.Play();
    }

    public void FadeOut()
    {
        anim.SetTrigger("FadeOut");
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
