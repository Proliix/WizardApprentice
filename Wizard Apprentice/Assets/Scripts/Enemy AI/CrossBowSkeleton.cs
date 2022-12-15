using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossBowSkeleton : MonoBehaviour, IStunnable
{
    enum AttackState { Idle, Walking, Shooting }
    [SerializeField] float walkSpeed = 2;
    [SerializeField] float shootCooldown = 0.15f;
    [SerializeField] float waitAfterShoot = 0.25f;
    [SerializeField] AttackState state;
    [Header("Bullets")]
    [SerializeField] float spawnDistance = 1;
    [SerializeField] float spawnDeviation = 1;


    float timer;
    Vector2 roomBoundary;
    Rigidbody2D rb2d;
    Animator anim;
    bool stunned = false;

    bool waitActive = false;
    BulletHandler bulletHandler;
    GameObject player;

    GameObject aimer;
    Vector3 targetPos;
    Vector3 dir = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        roomBoundary = GameObject.FindWithTag("GameController").GetComponent<RoomManager>().currentRoom.roomSize;
        bulletHandler = GameObject.FindWithTag("GameController").GetComponent<BulletHandler>();
        state = AttackState.Idle;
        aimer = new GameObject();
        aimer.transform.parent = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        anim.SetFloat("DirX", dir.normalized.x);
        anim.SetFloat("DirY", dir.normalized.y);


        switch (state)
        {
            case AttackState.Idle:
                Idle();
                break;
            case AttackState.Walking:
                if (!stunned)
                    Walking();
                break;
            case AttackState.Shooting:
                Shooting();
                break;
        }
    }

    void Idle()
    {
        state = AttackState.Walking;
        rb2d.velocity = Vector3.zero;
        float x = Random.Range(0 + 1, roomBoundary.x - 1);
        float y = Random.Range(0 + 1, roomBoundary.y - 1);
        targetPos = new Vector3(x, y, 0);
        dir = targetPos - gameObject.transform.position;
    }

    void Walking()
    {
        rb2d.velocity = dir.normalized * walkSpeed;
        Vector3 pos = transform.position;
        if ((pos.x > targetPos.x - 0.25f && pos.x < targetPos.x + 0.25f) && (pos.y > targetPos.y - 0.25f && pos.y < targetPos.y + 0.25f))
        {
            state = AttackState.Shooting;
        }
        dir = targetPos - pos;
    }

    void Shooting()
    {
        if (dir != Vector3.zero)
        {
            dir = Vector3.zero;
            rb2d.velocity = dir;
        }

        Vector3 pos = transform.position;
        Vector3 bulletDir = player.transform.position - pos;

        aimer.transform.position = pos + (bulletDir.normalized * spawnDistance);
        aimer.transform.up = bulletDir;

        if (timer >= shootCooldown)
        {
            bulletHandler.GetBullet(aimer.transform, gameObject, false, false, Vector3.zero, 10, 0.5f, 10);
            bulletHandler.GetBullet(aimer.transform, gameObject, false, true, Vector3.right * spawnDeviation, 10, 0.5f, 10);
            bulletHandler.GetBullet(aimer.transform, gameObject, false, true, Vector3.left * spawnDeviation, 10, 0.5f, 10);

            timer = 0;
        }

        if (!waitActive)
            StartCoroutine(ChangeStateAfterTime(waitAfterShoot, AttackState.Idle));
    }

    IEnumerator ChangeStateAfterTime(float time, AttackState newState)
    {
        waitActive = true;
        yield return new WaitForSeconds(time);
        state = newState;
        waitActive = false;
    }

    #region Stunned
    public void GetStunned(float stunDuration = 0.25F)
    {

        if (stunned)
            StopCoroutine(IsStunned(stunDuration));

        StartCoroutine(IsStunned(stunDuration));
    }

    public IEnumerator IsStunned(float stunDuration = 0.25F)
    {
        stunned = true;
        yield return new WaitForSeconds(stunDuration);
        stunned = false;
    }
    #endregion

    private void OnDrawGizmos()
    {
        switch (state)
        {
            case AttackState.Idle:
                break;
            case AttackState.Walking:
                Gizmos.DrawLine(transform.position, targetPos);
                Gizmos.DrawCube(targetPos, Vector3.one * 0.25f);
                break;
            case AttackState.Shooting:
                Gizmos.DrawCube(aimer.transform.position, Vector3.one * 0.25f);
                break;
        }
    }

}
