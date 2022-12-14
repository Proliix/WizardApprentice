using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossBowSkeleton : MonoBehaviour, IStunnable
{
    enum AttackState { Idle, Walking, Shooting }
    [SerializeField] float walkSpeed = 1;
    AttackState state;
    Vector2 roomBoundary;
    Rigidbody2D rb2d;
    Animator anim;
    bool stunned = false;

    bool newState;

    Vector3 targetPos;
    Vector3 dir = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        roomBoundary = GameObject.FindWithTag("GameController").GetComponent<RoomManager>().currentRoom.roomSize;
        state = AttackState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
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
                state = AttackState.Idle;
                break;
        }
    }

    void Idle()
    {
        targetPos.x = Random.Range(0 + 1, roomBoundary.x - 1);
        targetPos.y = Random.Range(0 + 1, roomBoundary.y - 1);
        state = AttackState.Walking;
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
                break;
        }
    }

}
