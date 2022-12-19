using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HauntedArmorBossManager : MonoBehaviour
{
    [SerializeField] Vector2 roomSize;
    [SerializeField] GameObject helmetObject;
    [SerializeField] GameObject firstGateObject;
    [SerializeField] GameObject secondGateObject;

    [SerializeField] Health polearmArmorHealth;
    [SerializeField] Health crossbowArmorHealth;
    [SerializeField] Health greatSwordArmorHealth;

    [SerializeField] GameObject polearmObject;
    [SerializeField] GameObject crossbowObject;
    [SerializeField] GameObject greatSwordObject;

    int currentPhase;


    [SerializeField] float timeToOpenGate;
    [SerializeField] float gateHeight;
    bool hasPressed;

    // Start is called before the first frame update
    void Start()
    {
        polearmArmorHealth.deathEvent += PolearmDeath;
        crossbowArmorHealth.deathEvent += CrossbowDeath;
        greatSwordArmorHealth.deathEvent += GreatSwordDeath;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            if (!hasPressed)
            {
                StartCoroutine(OpenGate(firstGateObject));
                hasPressed = true;
            }
            else
            {
                StartCoroutine(OpenGate(secondGateObject));
            }
        }
    }

    public void PolearmDeath(GameObject polearmObject)
    {
        if (currentPhase == 0)
        {
            currentPhase = 1;
            polearmObject.GetComponent<PolearmArmor>().enabled = false;
            StartCoroutine(OpenGate(firstGateObject));
            Invoke("SpawnCrossbow", 6f);
        }
    }

    public void CrossbowDeath(GameObject crossbowObject)
    {
        if (currentPhase == 1)
        {
            currentPhase = 2;
            crossbowObject.GetComponent<CrossbowArmor>().enabled = false;
            StartCoroutine(OpenGate(secondGateObject));
            Invoke("SpawnGreatSword", 6f);
        }
    }

    public void GreatSwordDeath(GameObject greatSwordObject)
    {
        if (currentPhase == 2)
        {
            greatSwordObject.GetComponent<GreatswordArmor>().enabled = false;
        }
    }

    public void SpawnCrossbow()
    {
        crossbowObject.GetComponent<CrossbowArmor>().enabled = true;
    }

    public void SpawnGreatSword()
    {
        greatSwordObject.GetComponent<GreatswordArmor>().enabled = true;
    }

    public IEnumerator OpenGate(GameObject gateObject)
    {
        float timeOpened = 0;
        Vector2 startPos = gateObject.transform.position;
        BoxCollider2D collider = gateObject.GetComponentInChildren<BoxCollider2D>();
        SpriteRenderer sprite = gateObject.GetComponentInChildren<SpriteRenderer>();
        while(timeOpened < timeToOpenGate)
        {
            yield return null;
            timeOpened += Time.deltaTime;
            sprite.gameObject.transform.position -= new Vector3(0,(Time.deltaTime/timeToOpenGate) * gateHeight,0);
        }
        collider.enabled = false;
        sprite.gameObject.transform.position = startPos - new Vector2(0,gateHeight);
    }
}
