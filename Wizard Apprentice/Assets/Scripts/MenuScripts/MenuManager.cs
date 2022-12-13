using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField] GameObject statsInGameCheckmark;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeInObjects());
        bool activeBool = PlayerPrefs.GetInt("StatInGame") > 0 ? true : false;
        statsInGameCheckmark.SetActive(activeBool);
    }

    // Update is called once per frame
    void Update()
    {

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


        statsInGameCheckmark.SetActive(!activeBool);
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
