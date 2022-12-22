using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{

    [SerializeField] AudioClip clickSound;
    private float clickVolume = 0.18f;
    [SerializeField] AudioClip howerSound;
    private float howerVolume = 0.06f;

    public void buttonClickSound()
    {
        SoundManager.Instance.PlayAudio(clickSound, clickVolume);
    }

    public void buttonHowerSound()
    {
        SoundManager.Instance.PlayAudio(howerSound, howerVolume);
    }
    
}
