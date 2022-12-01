using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathScreenController : MonoBehaviour
{
    [SerializeField] GameObject deathScreenObject;
    [SerializeField] Image blackScreenImage;
    [SerializeField] Animator retryButtonAnimator;
    [SerializeField] Animator menuButtonAnimator;
    [SerializeField] AnimationCurve deltaTimeRateOverTime;
    [SerializeField] float slowDownTime;
    [SerializeField] float timeBetweenAnimations;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            PlayerDeath();
        }
    }
    public void PlayerDeath()
    {
        deathScreenObject.SetActive(true);
        StartCoroutine(SlowDownTime());
        StartCoroutine(PlayAnimations());
    }
    public void RestartButtonClicked()
    {
        ResetTimeScale();
        SceneManager.LoadScene("Main");
    }

    public void MenuButtonClicked()
    {
        ResetTimeScale();
        SceneManager.LoadScene("Menu");
    }

    IEnumerator PlayAnimations()
    {
        yield return new WaitForSecondsRealtime(slowDownTime);
        retryButtonAnimator.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(timeBetweenAnimations);
        menuButtonAnimator.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(timeBetweenAnimations);
    }

    IEnumerator SlowDownTime()
    {
        Time.timeScale = 0.1f;
        float timePassed = 0;
        while (timePassed < slowDownTime)
        {
            yield return null;
            timePassed += Time.unscaledDeltaTime;
            blackScreenImage.color = new Color(0f, 0f, 0f, 0.5f - deltaTimeRateOverTime.Evaluate(timePassed / slowDownTime) * 0.5f);
        }
        Time.timeScale = 0;
        blackScreenImage.color = new Color(0f, 0f, 0f, (1f) * 0.5f);
    }

    private void ResetTimeScale()
    {
        Time.timeScale = 1f;
    }
}
