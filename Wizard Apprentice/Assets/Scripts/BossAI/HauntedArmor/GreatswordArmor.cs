using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEditor.PlayerSettings;

public class GreatswordArmor : MonoBehaviour
{

    GameObject playerObject;
    BulletHandler bulletHandler;

    CurrentState state;

    [SerializeField] GameObject spinToWinDamageObject;
    [SerializeField] float spinToWinDuration;
    [SerializeField] float spinToWinChargeTime;
    [SerializeField] float spinToWinRadius;
    [SerializeField] float spinToWinTimeBetweenProjectiles;
    [SerializeField] float spinToWinRotationsPerSecond;
    [SerializeField] float timeBetweenDamage;
    [SerializeField] float spinToWinRandomAngle;

    [SerializeField] GameObject slashAttackDamageObject;
    [SerializeField] float slashRange;
    [SerializeField] float slashAttackChargeDuration;
    [SerializeField] float slashAttackAngle;
    [SerializeField] int amountOfProjectiles;
    [SerializeField] int amountOfRectanglesForWarning;
    [SerializeField] float slashAttackProjectileSpeed;
    [SerializeField] float slashAttackRandomAngle;


    bool piecesOut = false;

    bool hasDestination;
    Vector2 currentDestination;
    [SerializeField] float movementSpeed;

    enum CurrentState
    {
        idle,
        handleSmash,
        spinning,
        slashing,
        splitting
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
        if (Input.GetKeyDown(KeyCode.U) && state == CurrentState.idle)
        {
            StartCoroutine(StartSpinning());
        }
        if (Input.GetKeyDown(KeyCode.I) && state == CurrentState.idle)
        {
            StartCoroutine(SlashAttack());
        }
        //currentDestination = playerObject.transform.position;
        //transform.position += ((Vector3)currentDestination - transform.position).normalized * movementSpeed * Time.deltaTime;
    }

    IEnumerator SlashAttack()
    {
        state = CurrentState.slashing;
        Vector2 startPos = transform.position;
        Vector2 targetPos = playerObject.transform.position;
        float randomAngle = Random.Range(-slashAttackRandomAngle, slashAttackRandomAngle);
        Vector2 warningDir = (targetPos - startPos);
        float angle = Mathf.Atan2(warningDir.y, warningDir.x);
        if (angle < 0.0)
            angle += Mathf.PI * 2;

        for (int i = 0; i < amountOfRectanglesForWarning; i++)
        {
            Vector2 newDir = new Vector2(Mathf.Cos((angle + (i * 2 * (slashAttackAngle/amountOfRectanglesForWarning) - slashAttackAngle) * Mathf.Deg2Rad)), Mathf.Sin(angle + (i * 2 * (slashAttackAngle / amountOfRectanglesForWarning) - slashAttackAngle) * Mathf.Deg2Rad));
            AttackIndicator.CreateSquare(startPos, startPos + newDir * slashRange, new Vector2(1,slashRange),slashAttackChargeDuration,true);
        }
        yield return new WaitForSeconds(slashAttackChargeDuration);

        List<GameObject> damageObjects = new List<GameObject>();
        for(int i = 0; i < amountOfRectanglesForWarning; i++)
        {
            Vector2 newDir = new Vector2(Mathf.Cos((angle + (i * 2 * (slashAttackAngle / amountOfRectanglesForWarning) - slashAttackAngle) * Mathf.Deg2Rad)), Mathf.Sin(angle + (i * 2 * (slashAttackAngle / amountOfRectanglesForWarning) - slashAttackAngle) * Mathf.Deg2Rad));
            Vector2 targetPosition = startPos + newDir * slashRange;
            float theta = Mathf.Atan2(targetPosition.y - startPos.y, startPos.x - targetPosition.x);
            if (theta < 0.0)
                theta += Mathf.PI * 2;
            GameObject slashObject = Instantiate(slashAttackDamageObject);
            slashObject.transform.localRotation = Quaternion.Euler(0, 0, (Mathf.Rad2Deg * theta - 90) * -1);
            slashObject.transform.position = startPos + newDir * slashRange * 0.5f;
            slashObject.transform.localScale = new Vector2(1,slashRange);
            damageObjects.Add(slashObject);
        }
        for (int i = 0; i < amountOfProjectiles; i++)
        {
            Vector2 newDir = new Vector2(Mathf.Cos((angle + (i * 2 * (slashAttackAngle / amountOfProjectiles) - slashAttackAngle) * Mathf.Deg2Rad)), Mathf.Sin(angle + (i * 2 * (slashAttackAngle / amountOfProjectiles) - slashAttackAngle) * Mathf.Deg2Rad));
            bulletHandler.GetBullet(startPos,newDir,false,10,0.5f,slashAttackProjectileSpeed);
        }
        state = CurrentState.idle;

        yield return new WaitForFixedUpdate();
        for (int i = 0; i < amountOfRectanglesForWarning; i++)
        {
            Destroy(damageObjects[i]);
        }

    }

    IEnumerator StartSpinning()
    {
        Debug.Log("Spinning");
        state = CurrentState.spinning;
        AttackIndicator.CreateCircle(transform.position, spinToWinRadius, spinToWinChargeTime, true);
        yield return new WaitForSeconds(spinToWinChargeTime);
        float currentSpinningTime = 0;
        CircleCollider2D circleCollider = Instantiate(spinToWinDamageObject).GetComponent<CircleCollider2D>();
        circleCollider.radius = spinToWinRadius / 2;
        while (currentSpinningTime < spinToWinDuration)
        {
            yield return null;
            circleCollider.transform.position = transform.position;
            if (Mathf.FloorToInt(currentSpinningTime / timeBetweenDamage) < Mathf.FloorToInt((currentSpinningTime + Time.deltaTime) / timeBetweenDamage))
            {
                circleCollider.enabled = true;
            }
            else if (Mathf.FloorToInt((currentSpinningTime + timeBetweenDamage * 0.5f) / timeBetweenDamage) < Mathf.FloorToInt(((currentSpinningTime + timeBetweenDamage * 0.5f) + Time.deltaTime) / timeBetweenDamage))
            {
                circleCollider.enabled = false;
            }
            int counter = 0;
            while (Mathf.FloorToInt((currentSpinningTime + (counter * spinToWinTimeBetweenProjectiles)) / spinToWinTimeBetweenProjectiles) < Mathf.FloorToInt((currentSpinningTime + Time.deltaTime) / spinToWinTimeBetweenProjectiles))
            {
                Vector3 dir = new Vector3(((Mathf.Cos((currentSpinningTime + (counter * spinToWinTimeBetweenProjectiles)) * spinToWinRotationsPerSecond) * 360 + Random.Range(-spinToWinRandomAngle, spinToWinRandomAngle)) * Mathf.Deg2Rad), ((Mathf.Sin((currentSpinningTime + (counter * spinToWinTimeBetweenProjectiles)) * spinToWinRotationsPerSecond) * 360 + Random.Range(-spinToWinRandomAngle, spinToWinRandomAngle)) * Mathf.Deg2Rad), 0);
                counter++;
                bulletHandler.GetBullet(transform.position, dir, false, 10, 0.5f, 8f);
            }
            currentSpinningTime += Time.deltaTime;
        }
        hasDestination = false;
        Destroy(circleCollider.gameObject);
        state = CurrentState.idle;
    }
}
