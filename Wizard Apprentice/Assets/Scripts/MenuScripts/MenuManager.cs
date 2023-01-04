using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Runtime.InteropServices;

public class MenuManager : MonoBehaviour
{
    [SerializeField] bool inMainMenu = true;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject backgroundObject;
    [SerializeField] Transform infrontParent;
    [SerializeField] Animator startButtonAnimator;
    [SerializeField] Animator settingsButtonAnimator;
    [SerializeField] Animator CreditsButtonAnimator;
    [SerializeField] Animator exitButtonAnimator;
    [SerializeField] Animator cameraAnimator;
    [Header("Credits")]
    [SerializeField] GameObject CreditsPanel;
    [SerializeField] GameObject licensePanel;
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
    [Header("For Debug")]
    [SerializeField] GameObject DebugPrefab;
    [Header("Ascension")]
    [SerializeField] GameObject ascensionButtonHolder;
    [SerializeField] GameObject ascensionButtonPrefab;
    [SerializeField] GameObject ascensionPanelObject;
    [SerializeField] TextMeshProUGUI ascensionEffectInfoText;
    [SerializeField] TextMeshProUGUI ascensionWinInfoText;
    [SerializeField] TextMeshProUGUI ascensionLoseInfoText;
    [SerializeField] TextMeshProUGUI totalCompletionsText;
    [SerializeField] TextMeshProUGUI highestAscensionText;
    [SerializeField] TextMeshProUGUI currentAscensionNumText;
    [SerializeField] float eloGainMultiplier;
    [SerializeField] float levelEloScaling;
    private int ascensionButtonsLoaded;
    private int ascensionRank;
    private int selectedLevel;

    MainMusicScript musicScript;
    bool onSettings;
    bool playSfxSound = false;
    int sfxFix = 0;
    GameObject debugObj;
    bool hasLoadedAscension = false;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("PlaySfxSoundOn", 1);
        musicScript = gameObject.GetComponent<MainMusicScript>();

        if (inMainMenu)
        {
            StartCoroutine(FadeInObjects());
            PlayerPrefs.SetInt("Debug", 0);
        }

        if (PlayerPrefs.GetInt("Debug") > 0)
        {
            Instantiate(DebugPrefab);
        }

        UpdateValues();
        settingsPanel.SetActive(false);
        onSettings = false;
        hasLoadedAscension = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            settingsPanel.SetActive(false);

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F6) && inMainMenu)
        {
            PlayerPrefs.DeleteAll();
            UpdateValues();
            Debug.Log("DeletedAll");
        }

        if (inMainMenu && Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.F12))
            {
                int debug = PlayerPrefs.GetInt("Debug") > 0 ? 0 : 1;
                PlayerPrefs.SetInt("Debug", debug);
                Debug.Log(debug);

                if (debug > 0)
                {
                    if (debugObj != null)
                        debugObj.SetActive(true);
                    else
                        debugObj = Instantiate(DebugPrefab);
                }
                else
                {
                    if (debugObj != null)
                        debugObj.SetActive(false);
                }

            }
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
        yield return new WaitForSeconds(0.25f);
        settingsButtonAnimator.SetTrigger("StartFadeIn");
        yield return new WaitForSeconds(0.25f);
        CreditsButtonAnimator.SetTrigger("StartFadeIn");
        yield return new WaitForSeconds(0.25f);
        exitButtonAnimator.SetTrigger("StartFadeIn");
    }

    public void StartGameButtonClicked()
    {
        if (PlayerPrefs.GetInt("Completions", 0) != 0)
        {
            ascensionPanelObject.SetActive(true);
            ascensionRank = PlayerPrefs.GetInt("ascensionRank", 0);
            totalCompletionsText.text = "Total Completions: " + PlayerPrefs.GetInt("Completions", 0).ToString();
            highestAscensionText.text = "Highest Ascension Defeated: " + ascensionRank.ToString();
            LoadInAscensionButtons(ascensionRank + 10);
            AscensionButtonClicked(0);
        }
        else
        {
            MoveIntoCave();
        }
    }
    private void LoadInAscensionButtons(int amount)
    {
        if (!hasLoadedAscension)
        {
            hasLoadedAscension = true;
            for (int i = 0; i < amount; i++)
            {
                GameObject buttonObject = Instantiate(ascensionButtonPrefab, ascensionButtonHolder.transform);
                int temp = i + ascensionButtonsLoaded + 1;
                buttonObject.GetComponent<Button>().onClick.AddListener(delegate { AscensionButtonClicked(temp); });
                buttonObject.GetComponentInChildren<TextMeshProUGUI>().text = $"Ascension Level {temp}";
                if (temp <= ascensionRank)
                {
                    buttonObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0, 1, 0);
                }
                else if (temp <= ascensionRank + 5)
                {
                    buttonObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color(1, 1, 1);
                }
                else
                {
                    buttonObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color(1, 0, 0);
                }
            }
            ascensionButtonHolder.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 80 * amount);
            ascensionButtonsLoaded += amount;
        }
    }

    public void AscensionButtonClicked(int levelNumber)
    {
        currentAscensionNumText.text = levelNumber.ToString();
        if (levelNumber == 0)
        {
            AscensionManager.selectedLevel = 0;
            AscensionManager.ascensionRank = ascensionRank;
            AscensionManager.gainOnWin = 0;
            AscensionManager.lossOnLose = 0;
            ascensionEffectInfoText.text = $"Enemies have <color=green>{100}% health";
            ascensionWinInfoText.text = $"<color=green> +{("0.00")} score";
            ascensionLoseInfoText.text = $"<color=red> -{("0.00")} score";
        }
        else if (levelNumber - 5 <= ascensionRank)
        {
            ascensionEffectInfoText.text = $"Enemies have <color=green>{levelNumber * 20 + 100}% health";
            float eloGain = 0;
            float levelDiff = levelNumber - ascensionRank;
            levelDiff *= levelEloScaling;
            eloGain = (1 / (1 + (Mathf.Pow(2, 0)))) - (1 / (1 + (Mathf.Pow(2, levelDiff)))) + 0.5f;
            if (levelDiff == 0)
            {
                eloGain = 0.5f * eloGainMultiplier;
            }
            else if (levelDiff > 0)
            {
                eloGain = Mathf.Log(levelDiff + 2, 4) * eloGainMultiplier;
            }
            else
            {
                eloGain = 1f / ((Mathf.Log((levelDiff * -1f) + 1, 2)) * eloGainMultiplier);
            }
            AscensionManager.selectedLevel = levelNumber;
            AscensionManager.ascensionRank = ascensionRank;
            AscensionManager.gainOnWin = eloGain;
            AscensionManager.lossOnLose = (1f / eloGain) * 0.66f;
            ascensionWinInfoText.text = $"<color=green> +{eloGain.ToString("0.00")} score";
            ascensionLoseInfoText.text = $"<color=red> -{((1f / eloGain) * 0.66f).ToString("0.00")} score";
            selectedLevel = levelNumber;
        }
        else
        {
            ascensionEffectInfoText.text = $"<color=red>You do not have access to this level yet. Defeat level {levelNumber - 5} or higher to gain access";
        }
    }

    public void AscensionStartGameClicked()
    {
        if (selectedLevel - 5 <= ascensionRank)
        {
            ascensionPanelObject.SetActive(false);
            MoveIntoCave();
        }
    }

    public void CloseAscensionPanel()
    {
        ascensionPanelObject.SetActive(false);
    }

    private void MoveIntoCave()
    {
        cameraAnimator.SetTrigger("MoveIntoCave");
        musicScript?.FadeOut();
        backgroundObject.transform.parent = infrontParent;
        Invoke("LoadMainScene", 1f);
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

    public void OpenLicenses()
    {
        licensePanel.SetActive(true);
        CreditsPanel.SetActive(false);
    }

    public void CloseLicenses()
    {
        licensePanel.SetActive(false);
        CreditsPanel.SetActive(true);
    }
    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }

    public void CreditsButtonPressed()
    {
        CreditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        CreditsPanel.SetActive(false);
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
