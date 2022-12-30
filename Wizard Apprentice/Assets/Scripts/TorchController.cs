using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

enum TorchType { Top, Right, Left, Bottom }
public class TorchController : MonoBehaviour
{
    [SerializeField] TorchType torchType;
    [SerializeField] float deviationSpeed = 0.7f;
    [SerializeField] float lightDeviation = 0.2f;
    [SerializeField] float rangeDeviation = 0.5f;

    Animator anim;
    float newIntencity;
    float newRange;
    float startIntencity;
    float startRange;

    bool movingDown = false;

    Light2D torchLight;

    // Start is called before the first frame update
    void Start()
    {
        torchLight = GetComponent<Light2D>();
        anim = GetComponent<Animator>();
        startIntencity = torchLight.intensity;
        startRange = torchLight.pointLightInnerRadius;
        switch (torchType)
        {
            case TorchType.Top:
                anim.SetInteger("Type", 0);
                break;
            case TorchType.Right:
                anim.SetInteger("Type", 1);
                break;
            case TorchType.Left:
                anim.SetInteger("Type", 2);
                break;
            case TorchType.Bottom:
                break;
        }

        GetNewTorchValues();
    }

    void GetNewTorchValues()
    {
        movingDown = !movingDown;
        //newIntencity = Random.Range(startIntencity - lightDeviation, startIntencity + lightDeviation);
        newIntencity = movingDown ? startIntencity - lightDeviation : startIntencity + lightDeviation;
        //newRange = Random.Range(startRange - rangeDeviation, startRange + lightDeviation);
        newRange = startRange / (startIntencity / newIntencity);
    }

    // Update is called once per frame
    void Update()
    {
        torchLight.intensity = Mathf.MoveTowards(torchLight.intensity, newIntencity, deviationSpeed * Time.deltaTime);
        torchLight.pointLightInnerRadius = Mathf.MoveTowards(torchLight.pointLightInnerRadius, newRange, deviationSpeed * Time.deltaTime);
        if (torchLight.intensity == newIntencity && torchLight.pointLightInnerRadius == newRange)
        {
            GetNewTorchValues();
        }
    }
}
