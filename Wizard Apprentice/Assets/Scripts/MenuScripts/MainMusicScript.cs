using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMusicScript : MonoBehaviour
{
    public AudioClip intro;
    public AudioClip loop;

    bool loopStarted = false;
    bool hasPassesHalf = false;
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
        //if (audioSource.time >= intro.length && !loopStarted)
        //{
        //    loopStarted = true;
        //    StartLoop();
        //}


        if (!loopStarted)
        {
            if (!hasPassesHalf)
            {
                if (audioSource.time > intro.length / 2)
                {
                    hasPassesHalf = true;
                }
            }
            else if (audioSource.time < intro.length / 2)
            {
                loopStarted = true;
                StartLoop();
            }
        }

    }

    void StartLoop()
    {
        audioSource.clip = loop;
        audioSource.loop = true;
        audioSource.Play();
    }

}
