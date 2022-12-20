using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] bool inMainMenu = true;
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
    [Header("Stats")]
    [SerializeField] Image statsInGameRenderer;
    [SerializeField] bool defaultStatValue;
    [Header("Dash")]
    [SerializeField] Image dashOverHeadRenderer;
    [SerializeField] bool defaultDashValue;
    [Header("Music")]
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider masterVolume;
    [SerializeField] Image masterImage;
    [SerializeField] Slider musicVolume;
    [SerializeField] Image musicImage;
    [SerializeField] Slider effectVolume;
    [SerializeField] Image effectImage;
    [Header("For Main")]
    [SerializeField] StatsUI statsUI;
    [SerializeField] GameObject dashOverHeadObj;
    [SerializeField] GameObject inactiveDashOverHeadObj;

    MainMusicScript musicScript;
    bool onSettings;
    bool playSfxSound = false;
    int sfxFix = 0;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("PlaySfxSoundOn", 1);
        musicScript = gameObject.GetComponent<MainMusicScript>();

        if (inMainMenu)
            StartCoroutine(FadeInObjects());

        UpdateValues();
        settingsPanel.SetActive(false);
        onSettings = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            settingsPanel.SetActive(false);

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F7) && inMainMenu)
        {
            PlayerPrefs.DeleteAll();
            UpdateValues();
            Debug.Log("DeletedAll");
        }
    }

    private void UpdateValues()
    {


            bool activeBool;
            if (PlayerPrefs.HasKey("StatsInGame"))
            {
                activeBool = PlayerPrefs.GetInt("StatsInGame") > 0 ? true : false;
            }
            else
            {
                activeBool = defaultStatValue;
                PlayerPrefs.SetInt("StatsInGame", activeBool ? 1 : 0);
            }
            statsInGameRenderer.sprite = activeBool ? checkMark : Cross;

            if (PlayerPrefs.HasKey("DashOverHead"))
            {
                activeBool = PlayerPrefs.GetInt("DashOverHead") > 0 ? true : false;
            }
            else
            {
                activeBool = defaultDashValue;
                PlayerPrefs.SetInt("DashOverHead", activeBool ? 1 : 0);
            }
            dashOverHeadRenderer.sprite = activeBool ? checkMark : Cross;

            float newValue = masterVolume.maxValue;
            masterVolume.maxValue = 0;
            masterVolume.minValue = -25;

            if (PlayerPrefs.HasKey("MasterVol"))
                newValue = PlayerPrefs.GetFloat("MasterVol");
            else
            {
                newValue = masterVolume.maxValue;
                PlayerPrefs.SetFloat("MasterVol", newValue);
            }

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
            if (PlayerPrefs.HasKey("MusicVol"))
                newValue = PlayerPrefs.GetFloat("MusicVol");
            else
            {
                newValue = musicVolume.maxValue;
                PlayerPrefs.SetFloat("MusicVol", newValue);
            }

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

            if (PlayerPrefs.HasKey("EffectVol"))
                newValue = PlayerPrefs.GetFloat("EffectVol");
            else
            {
                newValue = effectVolume.maxValue;
                PlayerPrefs.SetFloat("EffectVol", newValue);
            }

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

    public void ResetSettings()
    {
        PlayerPrefs.DeleteKey("StatsInGame");
        PlayerPrefs.DeleteKey("DashOverHead");
        PlayerPrefs.DeleteKey("MasterVol");
        PlayerPrefs.DeleteKey("MusicVol");
        PlayerPrefs.DeleteKey("EffectVol");
        UpdateValues();
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
        bool activeBool = PlayerPrefs.GetInt("StatsInGame") > 0 ? true : false;

        if (activeBool)
            PlayerPrefs.SetInt("StatsInGame", 0);
        else
            PlayerPrefs.SetInt("StatsInGame", 1);

        if (!inMainMenu)
            statsUI.UpdateShowStatus();

        statsInGameRenderer.sprite = activeBool ? Cross : checkMark;
        PlayerPrefs.Save();
    }

    public void EnableDashOverHead()
    {
        bool activeBool = PlayerPrefs.GetInt("DashOverHead") > 0 ? true : false;

        if (activeBool)
            PlayerPrefs.SetInt("DashOverHead", 0);
        else
            PlayerPrefs.SetInt("DashOverHead", 1);

        if (!inMainMenu)
        {
            dashOverHeadObj.SetActive(!activeBool);
            inactiveDashOverHeadObj.SetActive(!activeBool);
        }

        dashOverHeadRenderer.sprite = activeBool ? Cross : checkMark;
        PlayerPrefs.Save();
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }

    public void SettingsButtonClicked()
    {
        onSettings = true;
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        onSettings = false;
        settingsPanel.SetActive(false);
    }

    public void ExitButtonClicked()
    {
        Application.Quit();
    }
}
