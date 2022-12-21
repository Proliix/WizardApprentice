using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{

    [SerializeField] AudioClip clickSound;
    [SerializeField] float clickVolume = 1;
    [SerializeField] AudioClip howerSound;
    [SerializeField] float howerVolume = 1;

    public void buttonClickSound()
    {
        SoundManager.Instance.PlayAudio(clickSound, clickVolume);
    }

    public void buttonHowerSound()
    {
        SoundManager.Instance.PlayAudio(howerSound, howerVolume);
    }
    
}
