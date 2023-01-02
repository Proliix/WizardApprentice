using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Screenshake")]
    [SerializeField] float shakeDuration = 1;
    [SerializeField] float shakeSpeed = 10;
    [SerializeField] Vector2 shakeAmount = new Vector2(1, 1);
    [SerializeField] AnimationCurve curve = AnimationCurve.EaseInOut(0, 1, 1, 0);


    float time;
    bool shakeActive = false;
    bool debugMode = false;
    //Vector3 lastPos;
    //Vector3 nextPos;

    /// <summary>
    /// The ammount of running screenshake corutines
    /// </summary>
    int isRunning = 0;

    // Start is called before the first frame update
    void Start()
    {
        debugMode = PlayerPrefs.GetInt("Debug") > 0 ? true : false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && debugMode)
        {
            //shakeActive = true;
            time = shakeDuration;
            GetScreenShake(shakeDuration, shakeAmount);
        }
    }

    /// <summary>
    /// Starts screenshake with the default values which is defined on the camera
    /// </summary>
    public void GetScreenShake()
    {
        StartCoroutine(Shake(shakeDuration, shakeAmount));
    }

    /// <summary>
    /// Starts screenshake with duration with constrains vector2.one * amount
    /// </summary>
    public void GetScreenShake(float duration, float amount, bool withCurve = true)
    {
        if (withCurve)
            StartCoroutine(Shake(duration, Vector2.one * amount));
        else
            StartCoroutine(ShakeNoCurve(duration, Vector2.one * amount));


    }

    /// <summary>
    /// Starts screenshake with duration with constrains amount
    /// </summary>
    public void GetScreenShake(float duration, Vector2 amount, bool withCurve = true)
    {
        if (withCurve)
            StartCoroutine(Shake(duration, amount));
        else
            StartCoroutine(ShakeNoCurve(duration, amount));
    }

    IEnumerator Shake(float duration, Vector2 amount)
    {
        isRunning++;
        float elapsed = duration;
        Vector3 lastPos = Vector3.zero;
        Vector3 nextPos = Vector3.zero;
        while (elapsed > 0)
        {
            elapsed -= Time.deltaTime;
            nextPos = (Mathf.PerlinNoise(elapsed * shakeSpeed, elapsed * shakeSpeed * 2) - 0.5f) * amount.x * transform.right * curve.Evaluate(1f - elapsed / duration) +
                             (Mathf.PerlinNoise(elapsed * shakeSpeed * 2, elapsed * shakeSpeed) - 0.5f) * amount.y * transform.up * curve.Evaluate(1f - elapsed / duration);

            gameObject.transform.Translate(nextPos - lastPos);
            lastPos = nextPos;

            yield return null;
        }
        isRunning--;
        ResetCam();

    }

    IEnumerator ShakeNoCurve(float duration, Vector2 amount)
    {
        isRunning++;
        float elapsed = duration;
        Vector3 lastPos = Vector3.zero;
        Vector3 nextPos = Vector3.zero;
        while (elapsed > 0)
        {
            elapsed -= Time.deltaTime;
            nextPos = (Mathf.PerlinNoise(elapsed * shakeSpeed, elapsed * shakeSpeed * 2) - 0.5f) * amount.x * transform.right +
                             (Mathf.PerlinNoise(elapsed * shakeSpeed * 2, elapsed * shakeSpeed) - 0.5f) * amount.y * transform.up;

            gameObject.transform.Translate(nextPos - lastPos);
            lastPos = nextPos;

            yield return null;
        }
        isRunning--;
        ResetCam();

    }

    private void ResetCam()
    {
        if (isRunning <= 0)
        {
            transform.localPosition = new Vector3(0, 0, transform.localPosition.z);
        }

        //lastPos = nextPos = Vector3.zero;

        shakeActive = false;
    }
}