using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MinibossTeleportingSkeletonAI : MonoBehaviour
{
    [SerializeField] ParticleSystem teleportParticleEffect;
    [SerializeField] Light2D light2D;

    BulletHandler bulletHandler;
    Rigidbody2D rb2d;
    public Vector3 movePos;
    [SerializeField] float indicatorRadius = 50;
    float indicatorTime = 0.5f;


    [Header("Attack Variables")]
    [SerializeField] float bulletSize;
    [SerializeField] float damage;
    [SerializeField] float bulletSpeed;
    [SerializeField] float numberOfShotsInPattern = 4;
    [SerializeField] bool hasInvPhase = false;
    [SerializeField] int timesTillInv;
    [SerializeField] float timeInPhase = 5f;
    [SerializeField] float shootDelayPhase = 0.1f;
    [SerializeField] float angleChangeAmount = 0.1f;

    [Header("")]
    [SerializeField] float specialBulletSize;
    [SerializeField] float specialBulletSpeed;
    [SerializeField] float specialDamage;
    [SerializeField] int specialBulletAmount;

    [Header("Teleport Variables")]
    [SerializeField] float timeBetweenShots = 0.2f;
    [SerializeField] bool canMove;
    [SerializeField] float timer;
    [SerializeField] float moveDelay;

    private int lastNumber;
    Vector3 parentPos;
    GameObject parent;
    int timesJumped;
    bool invPhaseStarted = false;
    bool hasInicializedInv = false;
    float currentRightAngle;
    float currentLeftAngle;

    RoomManager roomManager;

    [SerializeField] GameObject target;



    void Start()
    {
        canMove = true;
        roomManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<RoomManager>();
        bulletHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<BulletHandler>();
        MoveEnemy();
        target = GameObject.FindWithTag("Player");
        parentPos = gameObject.transform.parent.position;
        parent = gameObject.transform.parent.gameObject;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (hasInvPhase)
        {
            if (timer >= moveDelay && canMove == true && !invPhaseStarted && timesJumped <= timesTillInv)
            {
                timesJumped++;
                timer -= moveDelay;

                StartCoroutine(MoveEnemy());
            }
            else if (!invPhaseStarted && !hasInicializedInv && timesJumped >= timesTillInv)
            {
                StopAllCoroutines();
                StartCoroutine(InitializeInvPhase());
            }
            else if (invPhaseStarted && timer >= shootDelayPhase)
            {
                timer = 0;
                currentRightAngle += angleChangeAmount;
                currentLeftAngle -= angleChangeAmount;
                bulletHandler.GetCircleShot(4, parent, false, currentRightAngle, 10, 0.5f, 10);
                bulletHandler.GetCircleShot(4, parent, false, currentLeftAngle, 10, 0.5f, 10);
            }

        }
        else
        {

            if (timer >= moveDelay && canMove == true)
            {
                timer -= moveDelay;

                StartCoroutine(MoveEnemy());
            }
        }

    }

    IEnumerator MoveEnemy()
    {


        canMove = false;
        int currentNumber = Random.Range(1, 5);

        while (currentNumber == lastNumber)
        {
            currentNumber = Random.Range(1, 5);

        }

        lastNumber = currentNumber;

        switch (currentNumber)
        {
            case 1:

                movePos = new Vector3(-5, 5, 0);

                break;

            case 2:
                movePos = new Vector3(5, 5, 0);

                break;

            case 3:
                movePos = new Vector3(5, -5, 0);

                break;

            case 4:
                movePos = new Vector3(-5, -5, 0);

                break;

        }

        TeleportEffect();
        yield return new WaitForSeconds(1f);


        //Boss isn't visable
        GoInvisable();
        yield return new WaitForSeconds(1f);

        TeleportIndicator();
        yield return new WaitForSeconds(indicatorTime);

        //Bos is visable
        gameObject.transform.localPosition = movePos;

        StartCoroutine(SpecialAttack());
        yield return new WaitForSeconds(1);

        StartCoroutine(AttackPattern());

    }

    IEnumerator InitializeInvPhase()
    {
        hasInicializedInv = true;
        currentRightAngle = -90 - angleChangeAmount;
        currentLeftAngle = 0 + angleChangeAmount;
        TeleportEffect();
        yield return new WaitForSeconds(1f);
        GoInvisable();
        AttackIndicator.CreateSquare(parentPos, new Vector2(parentPos.x + (roomManager.currentRoom.roomSize.x / 2), parentPos.y), new Vector2(1, roomManager.currentRoom.roomSize.x / 2), 1.5f, true);
        AttackIndicator.CreateSquare(parentPos, new Vector2(parentPos.x - (roomManager.currentRoom.roomSize.x / 2), parentPos.y), new Vector2(1, roomManager.currentRoom.roomSize.x / 2), 1.5f, true);
        AttackIndicator.CreateSquare(parentPos, new Vector2(parentPos.x, parentPos.y + (roomManager.currentRoom.roomSize.y / 2)), new Vector2(1, roomManager.currentRoom.roomSize.x / 2), 1.5f, true);
        AttackIndicator.CreateSquare(parentPos, new Vector2(parentPos.x, parentPos.y - (roomManager.currentRoom.roomSize.y / 2)), new Vector2(1,roomManager.currentRoom.roomSize.x / 2), 1.5f, true);
        yield return new WaitForSeconds(1.5f);
        gameObject.transform.localPosition = Vector3.zero;
        invPhaseStarted = true;
        yield return new WaitForSeconds(timeInPhase);
        timesJumped = 0;
        canMove = true;
        hasInicializedInv = false;
        invPhaseStarted = false;
    }

    private void BasicAttack()
    {
        bulletHandler.GetBullet(gameObject.transform.position, (target.transform.position - gameObject.transform.position).normalized, false, damage, bulletSize, bulletSpeed);

    }
    IEnumerator SpecialAttack()
    {
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(0.15f);
            bulletHandler.GetCircleShot(specialBulletAmount, gameObject, false, i * 25, specialDamage, specialBulletSize, specialBulletSpeed);
        }
    }


    IEnumerator AttackPattern()
    {
        for (int i = 0; i < numberOfShotsInPattern; i++)
        {
            BasicAttack();
            yield return new WaitForSeconds(timeBetweenShots);

        }

        canMove = true;
        yield return new WaitForSeconds(2f);

    }

    private void TeleportIndicator()
    {
        AttackIndicator.CreateCircle(parentPos + movePos, indicatorRadius, indicatorTime, true);
    }



    private void GoInvisable()
    {
        light2D.enabled = false;
        gameObject.transform.position = new Vector3(100, 100, 0);

    }

    private void TeleportEffect()
    {
        light2D.enabled = true;
        teleportParticleEffect.Play();

    }

}
