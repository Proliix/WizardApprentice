using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HauntedArmorBossManager : MonoBehaviour
{
    [SerializeField] Vector2 roomSize;
    BulletHandler bulletHandler;
    [SerializeField] GameObject firstGateObject;
    [SerializeField] GameObject secondGateObject;
    [SerializeField] GameObject firstGateHiderObject;
    [SerializeField] GameObject secondGateHiderObject;

    [SerializeField] GameObject polearmHealthbar;
    [SerializeField] GameObject crossBowHealthbar;
    [SerializeField] GameObject greatswordHealthbar;

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
        bulletHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<BulletHandler>();
        polearmArmorHealth.deathEvent += PolearmDeath;
        crossbowArmorHealth.deathEvent += CrossbowDeath;
        crossbowArmorHealth.enabled = false;
        greatSwordArmorHealth.deathEvent += GreatSwordDeath;
        greatSwordArmorHealth.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PolearmDeath(GameObject polearmObject)
    {
        if (currentPhase == 0)
        {
            currentPhase = 1;
            polearmObject.GetComponent<PolearmArmor>().enabled = false;
            StartCoroutine(OpenGate(firstGateObject, firstGateHiderObject));
            bulletHandler.ResetAll();
            Invoke("SpawnCrossbow", 5f);
            Destroy(polearmHealthbar);
            Destroy(polearmObject);
        }
    }

    public void CrossbowDeath(GameObject crossbowObject)
    {
        if (currentPhase == 1)
        {
            currentPhase = 2;
            crossbowObject.GetComponent<CrossbowArmor>().enabled = false;
            StartCoroutine(OpenGate(secondGateObject, secondGateHiderObject));
            bulletHandler.ResetAll();
            Invoke("SpawnGreatSword", 5f);
            Destroy(crossBowHealthbar);
            Destroy(crossbowObject);
        }
    }

    public void GreatSwordDeath(GameObject greatSwordObject)
    {
        if (currentPhase == 2)
        {
            greatSwordObject.GetComponent<GreatswordArmor>().Death();
            greatSwordObject.GetComponent<GreatswordArmor>().enabled = false;
            bulletHandler.ResetAll();
            Destroy(greatswordHealthbar);
            Destroy(greatSwordObject);
        }
    }

    public void SpawnCrossbow()
    {
        crossbowObject.GetComponent<CrossbowArmor>().enabled = true;
        crossbowArmorHealth.enabled = true;
    }

    public void SpawnGreatSword()
    {
        greatSwordObject.GetComponent<GreatswordArmor>().enabled = true;
        greatSwordArmorHealth.enabled = true;
    }

    public IEnumerator OpenGate(GameObject gateObject, GameObject gateHider)
    {
        float timeOpened = 0;
        Vector2 startPos = gateObject.transform.position;
        BoxCollider2D collider = gateObject.GetComponentInChildren<BoxCollider2D>();
        SpriteRenderer sprite = gateObject.GetComponentInChildren<SpriteRenderer>();
        while(timeOpened < timeToOpenGate)
        {
            yield return null;
            timeOpened += Time.deltaTime;
            gateHider.transform.position -= new Vector3(0, (Time.deltaTime / timeToOpenGate) * gateHeight * 0.5f, 0);
            gateHider.transform.localScale += new Vector3(0, (Time.deltaTime / timeToOpenGate) * gateHeight);
            sprite.gameObject.transform.position -= new Vector3(0,(Time.deltaTime/timeToOpenGate) * gateHeight,0);
        }
        collider.enabled = false;
        sprite.gameObject.transform.position = startPos - new Vector2(0,gateHeight);
    }
}
