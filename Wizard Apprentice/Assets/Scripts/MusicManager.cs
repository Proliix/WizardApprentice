using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MusicType { Normal, Boss, Event, Treasure, Map }
public class MusicManager : MonoBehaviour
{
    [SerializeField] GameObject musicObj1;
    [SerializeField] GameObject musicObj2;
    [Header("Normal")]
    [SerializeField] AudioClip normalIntro;
    [SerializeField] AudioClip normalLoop;
    [Header("Boss")]
    [SerializeField] AudioClip bossIntro;
    [SerializeField] AudioClip bossLoop;
    [Header("Event")]
    [SerializeField] AudioClip eventIntro;
    [SerializeField] AudioClip eventLoop;
    [Header("Treasure")]
    [SerializeField] AudioClip treasureIntro;
    [SerializeField] AudioClip treasureLoop;
    [Header("Map")]
    [SerializeField] AudioClip mapIntro;
    [SerializeField] AudioClip mapLoop;

    AudioSource audioSource1;
    AudioSource audioSource2;
    Animator anim1;
    Animator anim2;

    MusicType musicType;
    AudioClip currentIntro;
    AudioClip currentLoop;
    bool isAudioSource1 = true;
    bool currentLoopStarted = false;

    public static MusicManager Instance;


    //Singleton instance 
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        musicType = MusicType.Normal;
        audioSource1 = musicObj1.GetComponent<AudioSource>();
        audioSource2 = musicObj2.GetComponent<AudioSource>();
        anim1 = musicObj1.GetComponent<Animator>();
        anim2 = musicObj2.GetComponent<Animator>();
        currentIntro = normalIntro;
        currentLoop = normalLoop;
        isAudioSource1 = true;
        audioSource1.loop = false;
        audioSource2.loop = false;
        audioSource1.clip = currentIntro;
        audioSource1.Play();
        anim1.SetTrigger("FadeIn");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            ChangeToMusicType(musicType + 1);
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            ChangeToMusicType(musicType - 1);
        }

        if (currentLoop == null)
        {
            if (isAudioSource1)
                audioSource1.loop = true;
            else
                audioSource2.loop = true;

            currentLoopStarted = true;
        }

        if (audioSource1.time >= currentIntro.length && !currentLoopStarted && isAudioSource1)
        {
            Debug.Log("Is 1");
            currentLoopStarted = true;
            StartCurrentLoop();
        }
        else if (audioSource2.time >= currentIntro.length && !currentLoopStarted && !isAudioSource1)
        {
            Debug.Log("Is 2");
            currentLoopStarted = true;
            StartCurrentLoop();
        }
    }

    void StartCurrentLoop()
    {
        if (isAudioSource1)
        {
            audioSource1.clip = currentLoop;
            audioSource1.loop = true;
            audioSource1.Play();
        }
        else
        {
            audioSource2.clip = currentLoop;
            audioSource2.loop = true;
            audioSource2.Play();
        }


    }

    public void ChangeToMusicType(MusicType newType)
    {
        if (newType != musicType)
        {
            musicType = newType;

            switch (musicType)
            {
                case MusicType.Normal:
                    currentIntro = normalIntro;
                    currentLoop = normalLoop;
                    break;
                case MusicType.Boss:
                    currentIntro = bossIntro;
                    currentLoop = bossLoop;
                    break;
                case MusicType.Event:
                    currentIntro = eventIntro;
                    currentLoop = eventLoop;
                    break;
                case MusicType.Treasure:
                    currentIntro = treasureIntro;
                    currentLoop = treasureLoop;
                    break;
                case MusicType.Map:
                    currentIntro = mapIntro;
                    currentLoop = mapLoop;
                    break;
            }

            if (!isAudioSource1)
            {
                audioSource1.clip = currentIntro;
                audioSource1.Play();
            }
            else
            {
                audioSource2.clip = currentIntro;
                audioSource2.Play();
            }
            isAudioSource1 = !isAudioSource1;
            currentLoopStarted = false;

            StopCoroutine(StopMusicAfterTime());
            StartCoroutine(StopMusicAfterTime());
        }
    }

    IEnumerator StopMusicAfterTime()
    {
        if (isAudioSource1)
        {
            anim2.SetTrigger("FadeOut");
            anim1.SetTrigger("FadeIn");

        }
        else
        {
            anim1.SetTrigger("FadeOut");
            anim2.SetTrigger("FadeIn");
        }

        yield return new WaitForSeconds(1f);
        if (isAudioSource1)
        {
            audioSource2.Stop();
            audioSource2.loop = false;
        }
        else
        {
            audioSource1.Stop();
            audioSource1.loop = false;
        }
    }

}