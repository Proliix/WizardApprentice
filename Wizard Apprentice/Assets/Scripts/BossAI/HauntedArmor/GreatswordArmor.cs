using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatswordArmor : MonoBehaviour
{
    GameObject playerObject;
    BulletHandler bulletHandler;
    [SerializeField] Vector2 roomSize;

    CurrentState state;

    List<GameObject> allPieces;
    bool piecesAreOut;
    bool piecesAreReady;

    [SerializeField] float pieceSpinDuration;
    [SerializeField] float pieceSpinChargeTime;
    [SerializeField] float pieceSpinTimeBetweenProjectiles;
    [SerializeField] float pieceSpinRotationsPerSecond;

    [SerializeField] float pieceCircleDuration;
    [SerializeField] float pieceCircleRotationsPerSecond;
    [SerializeField] float pieceCircleDistance;
    [SerializeField] float pieceCircleApproachRate;
    [SerializeField] float pieceCircleDistanceChangeSpeed;
    [SerializeField] float pieceCircleDistanceChangeAmount;

    [SerializeField] GameObject pieceObject;
    [SerializeField] float timeToSplit;
    [SerializeField] int amountOfPieces;
    [SerializeField] float distanceToBeWithin;
    [SerializeField] float pieceMoveSpeed;

    [SerializeField] float recallingTime;
    [SerializeField] float recallingMovementSpeed;
    [SerializeField] int pulseProjectileAmount;
    [SerializeField] float pulseProjectileSpeed;
    [SerializeField] float timeBetweenPulse;
    [SerializeField] int timesToPulse;
    [SerializeField] float pulseDistanceToStop;

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

    bool hasDestination;
    Vector2 currentDestination;
    [SerializeField] float movementSpeed;

    enum CurrentState
    {
        idle,
        handleSmash,
        spinning,
        slashing,
        splitting,
        recalling
    }

    // Start is called before the first frame update
    void Start()
    {
        allPieces = new List<GameObject>();
        bulletHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<BulletHandler>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    public void SetRoomSize(Vector2 size)
    {
        roomSize = size;
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
        if (Input.GetKeyDown(KeyCode.Y) && state == CurrentState.idle)
        {
            StartCoroutine(SplitPieces());
        }
        if (Input.GetKeyDown(KeyCode.T) && piecesAreReady)
        {
            StartCoroutine(SpinPieces());
        }
        if (Input.GetKeyDown(KeyCode.R) && piecesAreReady && state == CurrentState.idle)
        {
            StartCoroutine(PullInPieces());
        }
        if (Input.GetKeyDown(KeyCode.E) && piecesAreReady && state == CurrentState.idle)
        {
            StartCoroutine(CircleAroundBoss());
        }
        //currentDestination = playerObject.transform.position;
        //transform.position += ((Vector3)currentDestination - transform.position).normalized * movementSpeed * Time.deltaTime;
    }

    IEnumerator SpinPieces()
    {
        piecesAreReady = false;
        yield return new WaitForSeconds(pieceSpinChargeTime);
        float timeSpinning = 0;
        while (timeSpinning < pieceSpinDuration)
        {
            yield return null;
            timeSpinning += Time.deltaTime;
            for (int i = 0; i < allPieces.Count; i++)
            {
                int counter = 0;
                while (Mathf.FloorToInt((timeSpinning + (counter * pieceSpinTimeBetweenProjectiles)) / pieceSpinTimeBetweenProjectiles) < Mathf.FloorToInt((timeSpinning + Time.deltaTime) / pieceSpinTimeBetweenProjectiles))
                {
                    Vector3 dir = new Vector3(((Mathf.Cos((timeSpinning + (counter * pieceSpinTimeBetweenProjectiles)) * pieceSpinRotationsPerSecond) * 360) * Mathf.Deg2Rad), ((Mathf.Sin((timeSpinning + (counter * pieceSpinTimeBetweenProjectiles)) * pieceSpinRotationsPerSecond) * 360) * Mathf.Deg2Rad), 0);
                    counter++;
                    bulletHandler.GetBullet(allPieces[i].transform.position, dir, false, 10, 0.5f, 8f);
                    Debug.Log("helo");
                }
            }
        }
        piecesAreReady = true;
    }

    IEnumerator CircleAroundBoss()
    {
        piecesAreReady = false;
        float timeCircling = 0;
        while(timeCircling < pieceCircleDuration)
        {
            yield return null;
            float distanceUsed = pieceCircleDistance + (Mathf.Sin(timeCircling * pieceCircleDistanceChangeSpeed) * pieceCircleDistanceChangeAmount);
            timeCircling += Time.deltaTime;
            for(int i = 0; i < allPieces.Count; i++)
            {
                float angle = (((float)i / allPieces.Count) * 360) + ((timeCircling / pieceCircleRotationsPerSecond) * (360f/allPieces.Count));
                Vector2 targetDestination = (Vector2)transform.position + new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)) * distanceUsed;
                float movementThisFrame = pieceCircleApproachRate * pieceCircleRotationsPerSecond * pieceCircleDistance * Mathf.PI * 2* Time.deltaTime / allPieces.Count;
                Vector2 dir = (targetDestination - (Vector2)allPieces[i].transform.position).normalized;
                if(movementThisFrame < Vector2.Distance(targetDestination,allPieces[i].transform.position))
                {
                    allPieces[i].transform.position += (Vector3)dir * movementThisFrame;
                }
                else
                {
                    allPieces[i].transform.position = targetDestination;
                }
            }
        }
        piecesAreReady = true;
    }

    IEnumerator PullInPieces()
    {
        state = CurrentState.recalling;
        yield return new WaitForSeconds(recallingTime);

        int amountCompleted = 0;
        bool[] isDoneMoving = new bool[allPieces.Count];
        while (amountCompleted < isDoneMoving.Length)
        {
            yield return null;
            for (int i = allPieces.Count-1; i >= 0; i--)
            {
                if (Vector2.Distance(allPieces[i].transform.position, transform.position) < pulseDistanceToStop && !isDoneMoving[i])
                {
                    amountCompleted++;
                    isDoneMoving[i] = true;

                }
                else if (!isDoneMoving[i])
                {
                    allPieces[i].transform.position += (Vector3)((transform.position - allPieces[i].transform.position).normalized * Time.deltaTime * pieceMoveSpeed);
                }
            }
        }
        for (int i = allPieces.Count - 1; i >= 0; i--)
        {
            Destroy(allPieces[i]);
            allPieces.RemoveAt(i);
        }
        piecesAreOut = false;
        float timePassed = 0;
        int timesPulsed = 0;
        while(timePassed < timeBetweenPulse * timesToPulse)
        {
            yield return null;
            timePassed += Time.deltaTime;
            int counter = 0;
            while (Mathf.FloorToInt((timePassed + (counter * timeBetweenPulse)) / timeBetweenPulse) < Mathf.FloorToInt((timePassed + Time.deltaTime) / timeBetweenPulse))
            {
                counter++;
                bulletHandler.GetCircleShot(pulseProjectileAmount, gameObject, false,(timesPulsed % 2) *(180 / pulseProjectileAmount), 10, 0.5f, pulseProjectileSpeed);
                timesPulsed++;
            }
        }
        state = CurrentState.idle;

    }
    IEnumerator SplitPieces()
    {
        state = CurrentState.splitting;
        Vector2 startPos = transform.position;
        yield return new WaitForSeconds(timeToSplit);

        List<Vector2> targetPositions = new List<Vector2>();

        for (int i = 0; i < amountOfPieces; i++)
        {
            targetPositions.Add(new Vector2(Random.Range(0, roomSize.x), Random.Range(0, roomSize.y)));
        }

        for (int i = 0; i < amountOfPieces; i++)
        {
            GameObject piece = Instantiate(pieceObject);
            piece.transform.position = startPos;
            float theta = Mathf.Atan2(targetPositions[i].y - startPos.y, startPos.x - targetPositions[i].x);
            if (theta < 0.0)
                theta += Mathf.PI * 2;
            piece.transform.localRotation = Quaternion.Euler(0, 0, (Mathf.Rad2Deg * theta - 90) * -1);
            allPieces.Add(piece);
        }
        state = CurrentState.idle;
        piecesAreOut = true;
        int amountCompleted = 0;
        bool[] isDoneMoving = new bool[allPieces.Count];
        while (amountCompleted < allPieces.Count)
        {
            yield return null;
            for (int i = 0; i < allPieces.Count; i++)
            {
                if (Vector2.Distance(allPieces[i].transform.position, targetPositions[i]) < distanceToBeWithin && !isDoneMoving[i])
                {
                    amountCompleted++;
                    isDoneMoving[i] = true;
                }
                else if (!isDoneMoving[i])
                {
                    allPieces[i].transform.position += (Vector3)((targetPositions[i] - startPos).normalized * Time.deltaTime * pieceMoveSpeed);
                }
            }
        }
        piecesAreReady = true;

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
