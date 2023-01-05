using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PolearmArmor : MonoBehaviour
{
    GameObject playerObject;
    BulletHandler bulletHandler;
    [SerializeField] Animator animator;
    [SerializeField] Vector2 roomSize;

    [SerializeField] GameObject polearmArmorObject;
    [SerializeField] float pokeDuration;
    [SerializeField] float pokeDistance;
    [SerializeField] float pokeProjectileSpeed;
    [SerializeField] GameObject pokeObjectPrefab;

    [SerializeField] float spinDuration;
    [SerializeField] float startSpinningTime;
    [SerializeField] float spinRadius;
    [SerializeField] GameObject spinDamageObject;
    [SerializeField] float timeBetweenDamage;
    [SerializeField] float timeBetweenProjectiles;
    [SerializeField] float rotaionsPerSecond;
    [SerializeField] float randomAngle;

    [SerializeField] float smashDuration;
    [SerializeField] GameObject smashShockwaveObjectPrefab;
    [SerializeField] GameObject smashZoneObjectPrefab;
    [SerializeField] float smashZoneRadius;
    [SerializeField] float smashDistanceFromCenter;
    [SerializeField] float shockwaveWidth;
    [SerializeField] float shockwaveLength;
    [SerializeField] float shockwaveSpeed;
    [SerializeField] CurrentState state;
    bool hasDestination;
    Vector2 currentDestination;
    [SerializeField] float movementSpeed;
    [SerializeField] float distanceToCheckNewDestination;

    [SerializeField] Vector2 abilityVariety;
    float spinValueMultiplier = 1f;
    float smashValueMultiplier = 1f;
    float pokeValueMultiplier = 1f;
    float timeSinceLastSpin;
    float timeSinceLastSmash;
    float timeSinceLastPoke;
    [SerializeField] float valueNeededForSpin;
    [SerializeField] float valueNeededForSmash;
    [SerializeField] float valueNeededForPoke;
    [SerializeField] float abilityTimeDependance;
    [SerializeField] float randomDistanceFromPlayer;
    [SerializeField] float playerDetectionRange;

    enum CurrentState
    {
        idle,
        smashing,
        spinning,
        poking
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

        if (state == CurrentState.idle)
        {
            UpdateAnimationBasedOnDirection((Vector2)playerObject.transform.position - (Vector2)this.transform.position);
            timeSinceLastSpin += Time.deltaTime;
            timeSinceLastSmash += Time.deltaTime;
            timeSinceLastPoke += Time.deltaTime;
            float distanceToPlayer = (playerObject.transform.position - transform.position).magnitude;
            float spinValue = (1 + (timeSinceLastSpin * abilityTimeDependance)) * spinValueMultiplier * (1 + (1f/distanceToPlayer));
            float smashValue = (1 + (timeSinceLastSmash * abilityTimeDependance)) * smashValueMultiplier *  (1+ (1f / distanceToPlayer));
            float pokeValue = (1 + (timeSinceLastPoke * abilityTimeDependance)) * pokeValueMultiplier * (1 + (1f / distanceToPlayer));

            if(spinValue > valueNeededForSpin)
            {
                timeSinceLastSpin = 0;
                spinValueMultiplier = Random.Range(abilityVariety.x,abilityVariety.y);
                StartCoroutine(StartSpinning());
            }
            else if (smashValue > valueNeededForSmash)
            {
                timeSinceLastSmash = 0;
                smashValueMultiplier = Random.Range(abilityVariety.x,abilityVariety.y);
                StartCoroutine(SmashPolearm());
            }
            else if (pokeValue > valueNeededForPoke)
            {
                timeSinceLastPoke = 0;
                pokeValueMultiplier = Random.Range(abilityVariety.x,abilityVariety.y);
                StartCoroutine(PokeTowardsPlayer());
            }

            if(Vector2.Distance(playerObject.transform.position,(Vector2)transform.position) < playerDetectionRange)
            {
                currentDestination = playerObject.transform.position;
            }
            if (hasDestination)
            {
                transform.position += ((Vector3)currentDestination - transform.position).normalized * movementSpeed * Time.deltaTime;
                if(Vector2.Distance(transform.position,currentDestination) < distanceToCheckNewDestination)
                {
                    currentDestination = (Vector2)playerObject.transform.position + Random.insideUnitCircle * randomDistanceFromPlayer;
                    int iter = 0;
                    while(currentDestination.x > roomSize.x || currentDestination.x < 0 || currentDestination.y > roomSize.y || currentDestination.y < 0)
                    {
                        currentDestination = (Vector2)playerObject.transform.position + Random.insideUnitCircle * (randomDistanceFromPlayer + iter);
                        if(iter > 50)
                        {
                            currentDestination = playerObject.transform.position;
                            break;
                        }
                    }
                }
            }
            else
            {
                currentDestination = (Vector2)playerObject.transform.position + Random.insideUnitCircle * randomDistanceFromPlayer;
                int iter = 0;
                while (currentDestination.x > roomSize.x || currentDestination.x < 0 || currentDestination.y > roomSize.y || currentDestination.y < 0)
                {
                    currentDestination = (Vector2)playerObject.transform.position + Random.insideUnitCircle * (randomDistanceFromPlayer + iter);
                    if (iter > 50)
                    {
                        currentDestination = playerObject.transform.position;
                        break;
                    }
                }
                hasDestination = true;
            }
        }
    }

    IEnumerator SmashPolearm()
    {
        state = CurrentState.smashing;
        Vector2 playerDir = (playerObject.transform.position - polearmArmorObject.transform.position).normalized;
        Vector2 startPos = (Vector2)transform.position + playerDir * (smashDistanceFromCenter) * 0.5f;
        Vector2 playerDirFromCenter = ((Vector2)playerObject.transform.position - startPos).normalized;
        Vector2 playerPos = playerObject.transform.position;
        AttackIndicator.CreateCircle(startPos, smashZoneRadius,smashDuration, true);
        AttackIndicator.CreateSquare(startPos, startPos + playerDir*shockwaveLength, new Vector2(shockwaveWidth, shockwaveLength), smashDuration, true);
        animator.SetBool("isSmashing", true);
        yield return new WaitForSeconds(smashDuration);
        animator.SetBool("isSmashing", false);
        GameObject smashShockwaveObject = Instantiate(smashShockwaveObjectPrefab);
        Destroy(smashShockwaveObject, shockwaveLength / shockwaveSpeed);
        GameObject smashShockwaveSpriteObject = smashShockwaveObject.GetComponentInChildren<SpriteRenderer>().gameObject;
        GameObject smashShockwaveColliderObject = smashShockwaveObject.GetComponentInChildren<BoxCollider2D>().gameObject;
        smashShockwaveColliderObject.transform.position = startPos;
        smashShockwaveSpriteObject.transform.position = startPos;
        float theta = Mathf.Atan2(playerPos.y - transform.position.y, transform.position.x - playerPos.x);
        if (theta < 0.0)
            theta += Mathf.PI * 2;
        smashShockwaveSpriteObject.transform.localRotation = Quaternion.Euler(0, 0, ((Mathf.Rad2Deg * theta - 90) * -1));
        hasDestination = false;
        state = CurrentState.idle;
        float shockwaveTimeTraveled = 0;

        while(shockwaveTimeTraveled < shockwaveLength / shockwaveSpeed && smashShockwaveObject != null)
        {
            yield return null;
            shockwaveTimeTraveled += Time.deltaTime;
            smashShockwaveSpriteObject.transform.position = startPos + (0.5f * (playerDir * (shockwaveTimeTraveled / (shockwaveLength/shockwaveSpeed))) * shockwaveLength);
            smashShockwaveSpriteObject.GetComponent<SpriteRenderer>().size = new Vector2(shockwaveWidth, (shockwaveTimeTraveled / (shockwaveLength / shockwaveSpeed)) * shockwaveLength);
            smashShockwaveColliderObject.transform.position = startPos - (playerDir * 0.5f) + playerDir * shockwaveLength * (shockwaveTimeTraveled / (shockwaveLength / shockwaveSpeed));
        }
    }

    IEnumerator PokeTowardsPlayer()
    {
        Vector2 playerDir = (playerObject.transform.position - polearmArmorObject.transform.position).normalized;
        Vector2 startPos = polearmArmorObject.transform.position;
        Vector2 playerPos = playerObject.transform.position;
        AttackIndicator.CreateSquare(startPos,playerPos, new Vector2(1,pokeDistance), pokeDuration, true);
        state = CurrentState.poking;
        UpdateAnimationBasedOnDirection(playerDir);
        animator.SetBool("isPoking", true);
        yield return new WaitForSeconds(pokeDuration);
        GameObject pokeObject = Instantiate(pokeObjectPrefab);
        pokeObject.transform.position = startPos + playerDir * pokeDistance * 0.5f;
        float theta = Mathf.Atan2(playerPos.y - startPos.y, startPos.x - playerPos.x);
        if (theta < 0.0)
            theta += Mathf.PI * 2;
        pokeObject.transform.localRotation = Quaternion.Euler(0, 0, (Mathf.Rad2Deg * theta - 90) * -1);

        bulletHandler.GetBullet(startPos + playerDir*pokeDistance,playerDir,false,10,0.5f, pokeProjectileSpeed);
        hasDestination = false;
        animator.SetBool("isPoking", false);
        state = CurrentState.idle;
        Destroy(pokeObject, 1f);
    }

    IEnumerator StartSpinning()
    {
        state = CurrentState.spinning;
        UpdateAnimationBasedOnDirection(new Vector2(0, -1));
        Vector2 startPos = transform.position;
        AttackIndicator.CreateCircle(startPos, spinRadius, startSpinningTime, true);
        UpdateAnimationBasedOnDirection((Vector2)playerObject.transform.position - startPos);
        yield return new WaitForSeconds(startSpinningTime);
        animator.SetBool("isSpinning", true);
        float currentSpinningTime = 0;
        CircleCollider2D circleCollider = Instantiate(spinDamageObject).GetComponent<CircleCollider2D>();
        circleCollider.transform.position = startPos;
        circleCollider.radius = spinRadius / 2;
        Destroy(circleCollider.gameObject, spinDuration);
        while(currentSpinningTime < spinDuration)
        {
            yield return null;
            if(Mathf.FloorToInt(currentSpinningTime / timeBetweenDamage) < Mathf.FloorToInt((currentSpinningTime + Time.deltaTime) / timeBetweenDamage))
            {
                circleCollider.enabled = true;
            }
            else if(Mathf.FloorToInt((currentSpinningTime + timeBetweenDamage *0.5f) / timeBetweenDamage) < Mathf.FloorToInt(((currentSpinningTime + timeBetweenDamage * 0.5f) + Time.deltaTime) / timeBetweenDamage))
            {
                circleCollider.enabled = false;
            }
            int counter = 0;
            while (Mathf.FloorToInt((currentSpinningTime + (counter * timeBetweenProjectiles)) / timeBetweenProjectiles) < Mathf.FloorToInt((currentSpinningTime + Time.deltaTime) / timeBetweenProjectiles))
            {
                Vector3 dir = new Vector3(((Mathf.Cos((currentSpinningTime + (counter * timeBetweenProjectiles)) * rotaionsPerSecond) * 360 + Random.Range(-randomAngle, randomAngle)) * Mathf.Deg2Rad), ((Mathf.Sin((currentSpinningTime+(counter * timeBetweenProjectiles)) * rotaionsPerSecond) * 360 + Random.Range(-randomAngle, randomAngle)) * Mathf.Deg2Rad), 0);
                counter++;
                bulletHandler.GetBullet(startPos, dir, false, 10, 0.5f, 8f);
            }
            currentSpinningTime += Time.deltaTime;
        }
        hasDestination = false;
        animator.SetBool("isSpinning", false);
        state = CurrentState.idle;
    }

    public void UpdateAnimationBasedOnDirection(Vector2 dir)
    {
        bool yLargest = Mathf.Abs(dir.y) > Mathf.Abs(dir.x);
        if(!yLargest)
        {
            animator.SetFloat("DirX",Mathf.Sign(dir.x));
            animator.SetFloat("DirY",0);
        }
        else
        {
            animator.SetFloat("DirY", Mathf.Sign(dir.y));
            animator.SetFloat("DirX", 0);
        }
    }
}
