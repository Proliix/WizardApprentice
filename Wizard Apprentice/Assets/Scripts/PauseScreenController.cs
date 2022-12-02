using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseScreenController : MonoBehaviour
{
    [SerializeField] GameObject pauseScreenObject;
    float timeScaleBeforePause = 1;
    bool isOpen;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isOpen)
            {
                ClosePause();
            }
            else
            {
                OpenPause();
            }
        }
    }
    public void OpenPause()
    {
        isOpen = true;
        pauseScreenObject.SetActive(true);
        timeScaleBeforePause = Time.timeScale;
        Time.timeScale = 0;
    }

    public void ClosePause()
    {
        isOpen = false;
        pauseScreenObject.SetActive(false);
        Time.timeScale = timeScaleBeforePause;
    }
    public void MenuButtonClicked()
    {
        ResetTimeScale();
        SceneManager.LoadScene("Menu");
    }


    private void ResetTimeScale()
    {
        Time.timeScale = 1f;
    }
}
