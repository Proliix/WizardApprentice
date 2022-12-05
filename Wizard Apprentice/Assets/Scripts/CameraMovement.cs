using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Screenshake")]
    [SerializeField] float shakeDuration = 1;
    [SerializeField] float shakeSpeed;
    [SerializeField] Vector2 shakeAmount = new Vector2(1, 1);
    [SerializeField] AnimationCurve curve = AnimationCurve.EaseInOut(0, 1, 1, 0);


    float time;
    bool shakeActive = false;
    Vector3 lastPos;
    Vector3 nextPos;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            shakeActive = true;
            time = shakeDuration;
        }
    }

    private void FixedUpdate()
    {

        if (shakeActive)
        {
            time -= Time.deltaTime;
            if (time > 0)
            {
                nextPos = (Mathf.PerlinNoise(time * shakeSpeed, time * shakeSpeed * 2) - 0.5f) * shakeAmount.x * transform.right * curve.Evaluate(1f - time / shakeDuration) +
                             (Mathf.PerlinNoise(time * shakeSpeed * 2, time * shakeSpeed) - 0.5f) * shakeAmount.y * transform.up * curve.Evaluate(1f - time / shakeDuration);

                gameObject.transform.Translate(nextPos - lastPos);
                lastPos = nextPos;
            }
            else
            {
                ResetCam();
            }
        }
    }

    private void ResetCam()
    {
        transform.localPosition = new Vector3(0, 0, transform.localPosition.z);

        lastPos = nextPos = Vector3.zero;

        shakeActive = false;
    }


}
