using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject backgroundObject;
    [SerializeField] Transform infrontParent;
    [SerializeField] Animator startButtonAnimator;
    [SerializeField] Animator settingsButtonAnimator;
    [SerializeField] Animator exitButtonAnimator;
    [SerializeField] Animator cameraAnimator;
    [Header("Settings")]
    [SerializeField] Sprite checkMark;
    [SerializeField] Sprite Cross;
    [SerializeField] Sprite normalHandle;
    [SerializeField] Image statsInGameRenderer;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider masterVolume;
    [SerializeField] Image masterImage;
    [SerializeField] Slider musicVolume;
    [SerializeField] Image musicImage;
    [SerializeField] Slider effectVolume;
    [SerializeField] Image effectImage;

    MainMusicScript musicScript;
    bool playSfxSound = false;
    int sfxFix = 0;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("PlaySfxSoundOn", 1);
        musicScript = gameObject.GetComponent<MainMusicScript>();
        StartCoroutine(FadeInObjects());
        bool activeBool = PlayerPrefs.GetInt("StatInGame") > 0 ? true : false;
        settingsPanel.SetActive(false);
        statsInGameRenderer.sprite = activeBool ? checkMark : Cross;

        float newValue = masterVolume.maxValue;
        masterVolume.maxValue = 0;
        masterVolume.minValue = -25;
        newValue = PlayerPrefs.GetFloat("MasterVol");
        masterVolume.value = newValue;
        masterImage.fillAmount = 1 - (masterVolume.value / masterVolume.minValue);
        if (newValue == -80)
        {
            masterVolume.value = masterVolume.minValue;
            masterImage.fillAmount = 0;
            masterVolume.targetGraphic.gameObject.GetComponent<Image>().sprite = Cross;
        }
        else
        {
            masterVolume.value = newValue;
            masterImage.fillAmount = 1 - (masterVolume.value / masterVolume.minValue);
        }


        newValue = musicVolume.maxValue;
        musicVolume.maxValue = 0;
        musicVolume.minValue = -25;
        newValue = PlayerPrefs.GetFloat("MusicVol");
        musicVolume.value = newValue;
        musicImage.fillAmount = 1 - (masterVolume.value / masterVolume.minValue);
        if (newValue == -80)
        {
            musicVolume.value = musicVolume.minValue;
            musicImage.fillAmount = 0;
            musicVolume.targetGraphic.gameObject.GetComponent<Image>().sprite = Cross;
        }
        else
        {
            musicVolume.value = newValue;
            musicImage.fillAmount = 1 - (musicVolume.value / musicVolume.minValue);
        }


        newValue = effectVolume.maxValue;
        effectVolume.maxValue = 10;
        effectVolume.minValue = -15;
        newValue = PlayerPrefs.GetFloat("EffectVol");
        if (newValue == -80)
        {
            effectVolume.value = effectVolume.minValue;
            effectImage.fillAmount = 0;
            effectVolume.targetGraphic.gameObject.GetComponent<Image>().sprite = Cross;
        }
        else
        {
            effectVolume.value = newValue;
            effectImage.fillAmount = -((effectVolume.value - effectVolume.minValue) / -(effectVolume.maxValue - effectVolume.minValue));
        }


    }

    public void UpdateMixer(int type)
    {
        switch (type)
        {
            case 0:
                if (masterVolume.minValue == masterVolume.value)
                {
                    mixer.SetFloat("Master", -80);
                    PlayerPrefs.SetFloat("MasterVol", -80);
                    masterVolume.targetGraphic.gameObject.GetComponent<Image>().sprite = Cross;
                }
                else
                {
                    masterVolume.targetGraphic.gameObject.GetComponent<Image>().sprite = normalHandle;
                    mixer.SetFloat("Master", masterVolume.value);
                    PlayerPrefs.SetFloat("MasterVol", masterVolume.value);
                }
                masterImage.fillAmount = 1 - (masterVolume.value / masterVolume.minValue);
                break;
            case 1:
                if (musicVolume.minValue == musicVolume.value)
                {
                    mixer.SetFloat("Music", -80);
                    musicVolume.targetGraphic.gameObject.GetComponent<Image>().sprite = Cross;
                    PlayerPrefs.SetFloat("MusicVol", -80);
                }
                else
                {
                    musicVolume.targetGraphic.gameObject.GetComponent<Image>().sprite = normalHandle;
                    mixer.SetFloat("Music", musicVolume.value);
                    PlayerPrefs.SetFloat("MusicVol", musicVolume.value);
                }
                musicImage.fillAmount = 1 - (musicVolume.value / musicVolume.minValue);
                break;
            case 2:
                if (effectVolume.minValue == effectVolume.value)
                {
                    mixer.SetFloat("Effect", -80);
                    effectVolume.targetGraphic.gameObject.GetComponent<Image>().sprite = Cross;
                    PlayerPrefs.SetFloat("EffectVol", -80);
                }
                else
                {
                    effectVolume.targetGraphic.gameObject.GetComponent<Image>().sprite = normalHandle;
                    mixer.SetFloat("Effect", effectVolume.value);
                    PlayerPrefs.SetFloat("EffectVol", effectVolume.value);
                }

                effectImage.fillAmount = -((effectVolume.value - effectVolume.minValue) / -(effectVolume.maxValue - effectVolume.minValue));
                sfxFix++;
                StartCoroutine(PlaySoundWithDelay(sfxFix));
                break;
        }
        PlayerPrefs.Save();
    }

    IEnumerator PlaySoundWithDelay(int index)
    {
        yield return new WaitForSeconds(0.1f);
        if (index == sfxFix)
        {
            if (playSfxSound)
            {

                sfxFix = 0;
                sfxSource.Play();
            }
            else
                playSfxSound = true;
        }

    }

    IEnumerator FadeInObjects()
    {
        startButtonAnimator.SetTrigger("StartFadeIn");
        yield return new WaitForSeconds(0.5f);
        settingsButtonAnimator.SetTrigger("StartFadeIn");
        yield return new WaitForSeconds(0.5f);
        exitButtonAnimator.SetTrigger("StartFadeIn");
    }

    public void StartGameButtonClicked()
    {
        cameraAnimator.SetTrigger("MoveIntoCave");
        musicScript?.FadeOut();
        backgroundObject.transform.parent = infrontParent;
        Invoke("LoadMainScene", 1f);
        //Zoom in on cave opening
        //Maybe wait a bit
        //Switch scene
    }

    public void EnableStatsWhenPlaying()
    {
        bool activeBool = PlayerPrefs.GetInt("StatInGame") > 0 ? true : false;

        if (activeBool)
            PlayerPrefs.SetInt("StatInGame", 0);
        else
            PlayerPrefs.SetInt("StatInGame", 1);


        statsInGameRenderer.sprite = activeBool ? Cross : checkMark;
        PlayerPrefs.Save();
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }

    public void SettingsButtonClicked()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void ExitButtonClicked()
    {
        Application.Quit();
    }
}
