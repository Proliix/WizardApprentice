using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowArmor : MonoBehaviour
{
    BulletHandler bulletHandler;
    GameObject playerObject;
    [SerializeField] Sprite arrowImage;
    [SerializeField] float bigArrowHealth;
    [SerializeField] float bigArrowChargeTime;
    [SerializeField] float bigArrowSpeed;
    [SerializeField] float bigArrowWarningLength;
    [SerializeField] float bigArrowWarningWidth;

    [SerializeField] float trippleShotChargeTime;
    [SerializeField] float trippleShotAngleDiff;
    [SerializeField] float trippleShotArrowSpeed;

    CurrentState state;

    [SerializeField] float timeToChargeArrowRain;
    [SerializeField] GameObject arrowRainDamageObject;
    [SerializeField] float timeBetweenArrowRainDamage;
    [SerializeField] int arrowRainAmount;
    [SerializeField] float arrowRainRadius;
    [SerializeField] float arrowRainDistanceAroundPlayer;
    [SerializeField] float arrowRainDuration;

    [SerializeField] Vector2 abilityVariety;
    float trippleShotValueMultiplier = 1f;
    float arrowRainValueMultiplier = 1f;
    float bigArrowValueMultiplier = 1f;
    float timeSinceLastTrippleShot = 0;
    float timeSinceLastArrowRain = 0;
    float timeSinceLastBigArrow = 0;
    [SerializeField] float valueNeededForTrippleShot;
    [SerializeField] float valueNeededForArrowRain;
    [SerializeField] float valueNeededForBigArrow;
    [SerializeField] float abilityTimeDependance;
    [SerializeField] float playerScaredRange;
    [SerializeField] float playerWithinRange;
    [SerializeField] float distanceToCheckNewDestination;

    bool hasDestination;
    Vector2 currentDestination;
    [SerializeField] float movementSpeed;
    [SerializeField] float preferredRange;

    enum CurrentState
    {
        idle,
        bigArrow,
        arrowRain,
        trippleArrow
    }

    // Start is called before the first frame update
    void Start()
    {
        bulletHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<BulletHandler>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H) && state == CurrentState.idle)
        {
            StartCoroutine(ShootBigArrow());
        }

        if (Input.GetKeyDown(KeyCode.G) && state == CurrentState.idle)
        {
            StartCoroutine(ShootTrippleShot());
        }

        if (Input.GetKeyDown(KeyCode.F) && state == CurrentState.idle)
        {
            StartCoroutine(StartArrowRain());
        }

        if (state == CurrentState.idle)
        {
            timeSinceLastTrippleShot += Time.deltaTime;
            timeSinceLastArrowRain += Time.deltaTime;
            timeSinceLastBigArrow += Time.deltaTime;
            float distanceToPlayer = (playerObject.transform.position - transform.position).magnitude;
            float trippleShotValue = (1 + (timeSinceLastTrippleShot * abilityTimeDependance)) * trippleShotValueMultiplier;
            float arrowRainValue = (1 + (timeSinceLastArrowRain * abilityTimeDependance)) * arrowRainValueMultiplier;
            float bigArrowValue = (1 + (timeSinceLastBigArrow * abilityTimeDependance)) * bigArrowValueMultiplier;

            if (arrowRainValue > valueNeededForArrowRain)
            {
                timeSinceLastArrowRain = 0;
                arrowRainValueMultiplier = Random.Range(abilityVariety.x, abilityVariety.y);
                StartCoroutine(StartArrowRain());
            }
            else if (bigArrowValue > valueNeededForBigArrow)
            {
                timeSinceLastBigArrow = 0;
                bigArrowValueMultiplier = Random.Range(abilityVariety.x, abilityVariety.y);
                StartCoroutine(ShootBigArrow());
            }
            else if (trippleShotValue > valueNeededForTrippleShot)
            {
                timeSinceLastTrippleShot = 0;
                trippleShotValueMultiplier = Random.Range(abilityVariety.x, abilityVariety.y);
                StartCoroutine(ShootTrippleShot());
            }

            if (Vector2.Distance(playerObject.transform.position, (Vector2)currentDestination) < playerScaredRange || Vector2.Distance(playerObject.transform.position, (Vector2)transform.position) > playerWithinRange)
            {
                currentDestination = (Vector2)playerObject.transform.position + (Random.insideUnitCircle.normalized * preferredRange);
            }
            if (hasDestination)
            {
                transform.position += ((Vector3)currentDestination - transform.position).normalized * movementSpeed * Time.deltaTime;
                if (Vector2.Distance(transform.position, currentDestination) < distanceToCheckNewDestination)
                {
                    currentDestination = (Vector2)playerObject.transform.position + (Random.insideUnitCircle.normalized * preferredRange);
                }
            }
            else
            {
                currentDestination = (Vector2)playerObject.transform.position + (Random.insideUnitCircle.normalized * preferredRange);
                hasDestination = true;
            }
        }

    }

    IEnumerator ShootTrippleShot()
    {
        state = CurrentState.trippleArrow;
        Vector3 pos = transform.position;
        Vector3 playerPos = playerObject.transform.position;
        yield return new WaitForSeconds(trippleShotChargeTime);

        Vector2 arrowDir = (playerPos - pos);
        float angle = Mathf.Atan2(arrowDir.y, arrowDir.x);
        if (angle < 0.0)
            angle += Mathf.PI * 2;

        Vector2 newDir1 = new Vector2(Mathf.Cos((angle + trippleShotAngleDiff * Mathf.Deg2Rad)), Mathf.Sin(angle + trippleShotAngleDiff * Mathf.Deg2Rad));
        Vector2 newDir2 = new Vector2(Mathf.Cos((angle - trippleShotAngleDiff * Mathf.Deg2Rad)), Mathf.Sin(angle - trippleShotAngleDiff * Mathf.Deg2Rad));


        GameObject splittingArrow1 = bulletHandler.GetSpecialBullet(pos, this.gameObject, newDir1, arrowImage, SpecialBulletState.HauntedArmorSplittingArrow, null, false, 0, 300, 10, 1f, trippleShotArrowSpeed, 1);
        GameObject splittingArrow2 = bulletHandler.GetSpecialBullet(pos, this.gameObject, (playerPos - pos), arrowImage, SpecialBulletState.HauntedArmorSplittingArrow, null, false, 0, 300, 10, 1f, trippleShotArrowSpeed, 1);
        GameObject splittingArrow3 = bulletHandler.GetSpecialBullet(pos, this.gameObject, newDir2, arrowImage, SpecialBulletState.HauntedArmorSplittingArrow, null, false, 0, 300, 10, 1f, trippleShotArrowSpeed, 1);
        splittingArrow1.transform.right = -newDir1;
        splittingArrow2.transform.right = -arrowDir;
        splittingArrow3.transform.right = -newDir2;
        hasDestination = false;
        state = CurrentState.idle;
    }

    IEnumerator ShootBigArrow()
    {
        state = CurrentState.bigArrow;
        Vector2 startPos = transform.position;
        Vector2 playerDir = (playerObject.transform.position - transform.position).normalized;
        AttackIndicator.CreateSquare(startPos, startPos + (playerDir * bigArrowWarningLength), new Vector2(bigArrowWarningWidth, bigArrowWarningLength), bigArrowChargeTime, true);
        yield return new WaitForSeconds(bigArrowChargeTime);
        GameObject bigArrow = bulletHandler.GetSpecialBullet(startPos, this.gameObject, playerDir, arrowImage, SpecialBulletState.HauntedArmorBigArrow, null, false, 0, 300, 25, bigArrowWarningWidth, bigArrowSpeed, 1);
        bigArrow.GetComponent<SpecialProjectile>().data = new List<float>();
        bigArrow.GetComponent<SpecialProjectile>().data.Add(bigArrowHealth - 1);
        hasDestination = false;
        state = CurrentState.idle;
    }

    IEnumerator StartArrowRain()
    {
        state = CurrentState.arrowRain;

        List<Vector2> arrowRainPositions = new List<Vector2>();

        for(int i = 0; i < arrowRainAmount; i++)
        {
            Vector2 arrowRainPos = (Vector2)playerObject.transform.position + Random.insideUnitCircle * arrowRainDistanceAroundPlayer;
            AttackIndicator.CreateCircle(arrowRainPos, arrowRainRadius, timeToChargeArrowRain, true);
            arrowRainPositions.Add(arrowRainPos);
        }
        yield return new WaitForSeconds(timeToChargeArrowRain);
        state = CurrentState.idle;

        float arrowRainTime = 0;
        List<GameObject> circleColliders = new List<GameObject>();
        for(int i = 0; i < arrowRainAmount; i++)
        {
            GameObject arrowRainCollider = Instantiate(arrowRainDamageObject);
            arrowRainCollider.transform.position = arrowRainPositions[i];
            arrowRainCollider.transform.localScale = new Vector3(arrowRainRadius, arrowRainRadius, 1);
            circleColliders.Add(arrowRainCollider);
        }
        for (int i = 0; i < circleColliders.Count; i++)
        {
            Destroy(circleColliders[i].gameObject, arrowRainDuration);
        }
        while (arrowRainTime < arrowRainDuration)
        {
            yield return null;
            if (Mathf.FloorToInt(arrowRainTime / timeBetweenArrowRainDamage) < Mathf.FloorToInt((arrowRainTime + Time.deltaTime) / timeBetweenArrowRainDamage))
            {
                for(int i = 0; i < circleColliders.Count; i++)
                {
                    circleColliders[i].GetComponent<CircleCollider2D>().enabled = true;
                }
            }
            else if (Mathf.FloorToInt((arrowRainTime + timeBetweenArrowRainDamage * 0.5f) / timeBetweenArrowRainDamage) < Mathf.FloorToInt(((arrowRainTime + timeBetweenArrowRainDamage * 0.5f) + Time.deltaTime) / timeBetweenArrowRainDamage))
            {
                for (int i = 0; i < circleColliders.Count; i++)
                {
                    circleColliders[i].GetComponent<CircleCollider2D>().enabled = false;
                }
            }
            arrowRainTime += Time.deltaTime;
        }
        hasDestination = false;
        
    }
}
