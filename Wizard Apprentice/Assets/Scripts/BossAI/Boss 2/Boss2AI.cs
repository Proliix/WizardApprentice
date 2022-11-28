using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2AI : MonoBehaviour
{

    [Header("Phases")]
    [SerializeField] bool phase1;
    [SerializeField] bool phase2;
    [SerializeField] bool phase3;
    [SerializeField] bool isDead;

    [Header("Boss Pieces")]
    [SerializeField] GameObject boss2Piece1;
    [SerializeField] GameObject boss2Piece2;
    [SerializeField] GameObject boss2Piece3;

    [SerializeField] Boss2PiecesAI boss2PieceAll;

    [SerializeField] Health health;
    
    [SerializeField] private float HP;
    [SerializeField] private float maxHP;

    //References to shooting pieces script
    Boss2PiecesAI piece1;
    Boss2PiecesAI piece2;
    Boss2PiecesAI piece3;
    Boss2PiecesAI pieceAll;

    void Start()
    {
        health = gameObject.GetComponent<Health>();

        //Sets upp phases 
        phase1 = true;
        phase2 = false;
        phase3 = false;
        isDead = false;

        //Vars for simplicity
        piece1 = boss2Piece1.GetComponent<Boss2PiecesAI>();
        piece2 = boss2Piece2.GetComponent<Boss2PiecesAI>();
        piece3 = boss2Piece3.GetComponent<Boss2PiecesAI>();

        piece1.canShoot = true;

        

    }

    void Update()
    {

        HP = health.GetHP();
        maxHP = health.GetMaxHP();

        //Start phase 2
        if (HP < maxHP * 0.666f && phase1)
        {
            boss2Piece2.SetActive(true);
            phase1 = false;
            phase2 = true;
            piece2.canShoot = true;
        }

        //Start phase 3
        if (HP < maxHP * 0.333f && phase2)
        {
            boss2Piece3.SetActive(true);
            phase2 = false;
            phase3 = true;
            piece3.canShoot = true;
        }

        //Dies
        if (HP <= 50)
        {

            HP = 10;
            piece1.canShoot = false;
            piece2.canShoot = false;
            piece3.canShoot = false;
            isDead = true;
            StartCoroutine(death());
            

        }

        IEnumerator death()
        {
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
            yield return null;
        }

    }
}
