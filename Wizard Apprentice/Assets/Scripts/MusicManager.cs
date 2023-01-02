using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MusicType { Normal, Boss, Event, Treasure, Map }
public class MusicManager : MonoBehaviour
{
    [SerializeField] GameObject musicObj1;
    [SerializeField] GameObject musicObj2;
    [SerializeField] GameObject musicObjPause;
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
    [Header("Pause")]
    [SerializeField] AudioClip pauseLoop;

    AudioSource audioSource1;
    AudioSource audioSource2;
    AudioSource audioSourcePause;
    Animator anim1;
    Animator anim2;
    Animator animPause;

    MusicType musicType;
    AudioClip currentIntro;
    AudioClip currentLoop;
    bool isAudioSource1 = true;
    bool currentLoopStarted = false;
    bool hasStoppedLastPlayer = true;
    bool hasPassedHalfWay = false;
    bool onPauseScreen = false;
    bool hasStoppedPause = false;

    public static MusicManager Instance;

    bool debugMode;

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

        musicType = MusicType.Map;
        audioSource1 = musicObj1.GetComponent<AudioSource>();
        audioSource2 = musicObj2.GetComponent<AudioSource>();
        audioSourcePause = musicObjPause.GetComponent<AudioSource>();
        audioSourcePause.clip = pauseLoop;
        anim1 = musicObj1.GetComponent<Animator>();
        anim2 = musicObj2.GetComponent<Animator>();
        animPause = musicObjPause.GetComponent<Animator>();
        //currentIntro = normalIntro;
        //currentLoop = normalLoop;
        //isAudioSource1 = true;

        audioSource1.loop = true;
        audioSource2.loop = true;
        // audioSource1.clip = currentIntro;
        //audioSource1.Play();
        //anim1.SetTrigger("FadeIn");
        ChangeToMusicType(MusicType.Normal);
        debugMode = PlayerPrefs.GetInt("Debug") > 0 ? true : false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5) && musicType != MusicType.Map && debugMode)
        {
            ChangeToMusicType(musicType + 1);
        }
        else if (Input.GetKeyDown(KeyCode.F4) && musicType != MusicType.Normal && debugMode)
        {
            ChangeToMusicType(musicType - 1);
        }

        if (!hasStoppedLastPlayer)
        {
            if ((isAudioSource1 ? audioSource1.volume : audioSource2.volume) <= 0)
            {
                switch (isAudioSource1)
                {
                    case false:
                        audioSource1.Stop();
                        //audioSource1.loop = false;
                        break;
                    case true:
                        audioSource2.Stop();
                        //audioSource2.loop = false;
                        break;
                }
                hasStoppedLastPlayer = true;
            }
        }

        if (!hasStoppedPause)
        {

            if (onPauseScreen)
            {
                if ((!isAudioSource1 ? audioSource1.volume : audioSource2.volume) <= 0)
                {
                    switch (isAudioSource1)
                    {
                        case false:
                            audioSource1.Pause();
                            //audioSource1.loop = false;
                            break;
                        case true:
                            audioSource2.Pause();
                            //audioSource2.loop = false;
                            break;
                    }
                    hasStoppedPause = true;

                }
            }
            else
            {
                if (audioSourcePause.volume <= 0)
                {
                    audioSourcePause.Stop();
                    hasStoppedPause = true;
                }
            }
        }

        if (!currentLoopStarted)
        {



            if (audioSource1.time <= currentIntro.length / 2f && hasPassedHalfWay && isAudioSource1)
            {
                StartCurrentLoop();
            }
            else if (audioSource2.time <= currentIntro.length / 2f && hasPassedHalfWay && !isAudioSource1)
            {
                StartCurrentLoop();
            }

            if (audioSource1.time > currentIntro.length / 2 && isAudioSource1)
            {
                hasPassedHalfWay = true;
            }
            else if (audioSource2.time > currentIntro.length / 2 && !isAudioSource1)
            {
                hasPassedHalfWay = true;
            }

        }
    }

    void StartCurrentLoop()
    {
        currentLoopStarted = true;
        hasPassedHalfWay = false;
        if (isAudioSource1)
        {
            audioSource1.clip = currentLoop;
            //audioSource1.loop = true;
            audioSource1.Play();
        }
        else
        {
            audioSource2.clip = currentLoop;
            //audioSource2.loop = true;
            audioSource2.Play();
        }


    }

    public void AddPauseScreen()
    {
        onPauseScreen = true;
        hasStoppedPause = false;
        if (isAudioSource1)
        {
            anim1.SetTrigger("FadeOut");

        }
        else
        {
            anim2.SetTrigger("FadeOut");
        }

        audioSourcePause.Play();
        animPause.SetTrigger("FadeIn");
    }

    public void RemovePauseScreen()
    {
        onPauseScreen = false;
        hasStoppedPause = false;
        if (isAudioSource1)
        {
            anim1.SetTrigger("FadeIn");
            audioSource1.UnPause();
        }
        else
        {
            anim2.SetTrigger("FadeIn");
            audioSource2.UnPause();
        }
        animPause.SetTrigger("FadeOut");
    }

    public void ChangeToMusicType(MusicType newType)
    {
        if (newType != musicType)
        {
            musicType = newType;
            currentLoopStarted = false;

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

            isAudioSource1 = !isAudioSource1;
            currentLoopStarted = false;
            if (isAudioSource1)
            {
                if (currentIntro != null)
                {
                    audioSource1.clip = currentIntro;
                    audioSource1.Play();
                }
                else
                {
                    StartCurrentLoop();
                }
            }
            else
            {
                if (currentIntro != null)
                {
                    audioSource2.clip = currentIntro;
                    audioSource2.Play();
                }
                else
                {
                    StartCurrentLoop();
                }
            }


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

            hasStoppedLastPlayer = false;
        }
    }

}
