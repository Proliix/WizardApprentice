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
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider masterVolume;
    [SerializeField] Slider musicVolume;
    [SerializeField] Slider effectVolume;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeInObjects());
        bool activeBool = PlayerPrefs.GetInt("StatInGame") > 0 ? true : false;
        statsInGameRenderer.sprite = activeBool ? checkMark : Cross;

        float newValue = 0;
        masterVolume.maxValue = 0;
        masterVolume.minValue = -30;
        mixer.GetFloat("Master", out newValue);
        masterVolume.value = newValue;

        musicVolume.maxValue = 0;
        musicVolume.minValue = -30;
        mixer.GetFloat("Music", out newValue);
        musicVolume.value = newValue;

        effectVolume.maxValue = 0;
        effectVolume.minValue = -30;
        mixer.GetFloat("Effect", out newValue);
        effectVolume.value = newValue;


    }

    public void UpdateMixer(int type)
    {
        switch (type)
        {
            case 0:
                if (masterVolume.minValue == masterVolume.value)
                {
                    mixer.SetFloat("Master", -80);
                    masterVolume.targetGraphic.gameObject.GetComponent<Image>().sprite = Cross;
                }
                else
                {
                    masterVolume.targetGraphic.gameObject.GetComponent<Image>().sprite = normalHandle;
                    mixer.SetFloat("Master", masterVolume.value);
                }
                break;
            case 1:
                if (musicVolume.minValue == musicVolume.value)
                {
                    mixer.SetFloat("Music", -80);
                    musicVolume.targetGraphic.gameObject.GetComponent<Image>().sprite = Cross;
                }
                else
                {
                    musicVolume.targetGraphic.gameObject.GetComponent<Image>().sprite = normalHandle;
                    mixer.SetFloat("Music", musicVolume.value);
                }
                break;
            case 2:
                if (effectVolume.minValue == effectVolume.value)
                {
                    mixer.SetFloat("Effect", -80);
                    effectVolume.targetGraphic.gameObject.GetComponent<Image>().sprite = Cross;
                }
                else
                {
                    effectVolume.targetGraphic.gameObject.GetComponent<Image>().sprite = normalHandle;
                    mixer.SetFloat("Effect", effectVolume.value);
                }
                break;
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
