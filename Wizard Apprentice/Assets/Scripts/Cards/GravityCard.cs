using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityCard : MonoBehaviour, ICard
{
    [SerializeField] Sprite image;
    [SerializeField] Sprite Bulletimage;
    [SerializeField] float effectRange = 2f;


    bool hasFired = false;
    BulletHandler bulletHandler;
    EnemyManager enemyManager;
    GameObject player;
    GameObject gravityBullet;
    List<GameObject> enemiesWithingRange;

    private void Start()
    {
        bulletHandler = GameObject.FindWithTag("GameController").GetComponent<BulletHandler>();
        enemyManager = GameObject.FindWithTag("Gamecontroller").GetComponent<EnemyManager>();
        player = GameObject.FindWithTag("Player");
    }
    public void Effect()
    {
        enemiesWithingRange = enemyManager.GetEnemiesWithinRange(gravityBullet.transform.position, effectRange);
        throw new System.NotImplementedException();
    }

    public Sprite GetSprite()
    {
        return image;
    }

    public void ResetCard()
    {
        hasFired = false;
    }

    public void UpdateCard()
    {
        if (!hasFired)
        {
            hasFired = true;
            gravityBullet = bulletHandler.GetSpecialBullet(gameObject.transform.position, gameObject, Bulletimage, SpecialBulletState.Static, this, true);
            Effect();
        }
    }
}
