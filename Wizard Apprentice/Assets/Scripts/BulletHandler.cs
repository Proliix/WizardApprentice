using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    [SerializeField] GameObject normalProjectile;
    [SerializeField] int startSize = 25;
    [SerializeField] GameObject poolParent;
    List<GameObject> projectilePool;
    



    // Start is called before the first frame update
    void Start()
    {
        projectilePool = new List<GameObject>();

        for (int i = 0; i < startSize; i++)
        {
            projectilePool.Add(Instantiate(normalProjectile, poolParent.transform.position, normalProjectile.transform.rotation, poolParent.transform));
            projectilePool[i].SetActive(false);
            projectilePool[i].GetComponent<Bullet>().poolIndex = i;
            projectilePool[i].name = "Bullet: " + i;
        }
    }

    public void ResetBullet(int index)
    {
        projectilePool[index].SetActive(false);
        projectilePool[index].transform.position = poolParent.transform.position;
    }

    void SetUpBullet(GameObject poolMember, Vector3 position, GameObject shooter, bool isPlayer, bool moveAwayFromShooter)
    {
        poolMember.transform.position = position;
        Bullet bullet = poolMember.GetComponent<Bullet>();
        bullet.shooter = shooter;
        bullet.isPlayerBullet = isPlayer;
        bullet.moveAwayFromShoter = moveAwayFromShooter;
        bullet.UpdateColor();
        bullet.UpdateDirection();
        bullet.ResetTimer();
        poolMember.SetActive(true);
    }

    public void GetCircleShot(int size, GameObject shooter, bool isPlayer)
    {
        if (size <= 0)
            Debug.LogError("INVALID SIZE IN CIRCLESHOTFUNCTION MADE BY " + shooter.name);
        else
        {
            for (float deg = 0; deg < 360; deg += 360f / size)
            {
                float vertical = Mathf.Sin(Mathf.Deg2Rad * (deg + 90));
                float horizontal = Mathf.Cos(Mathf.Deg2Rad * (deg + 90));

                Vector3 spawnDir = new Vector3(horizontal, vertical, 0);

                Vector3 spawnPos = shooter.transform.position + spawnDir;

                GetBullet(spawnPos, shooter, isPlayer, true);
            }
        }
    }

    public void GetBullet(Vector3 position, GameObject shooter, bool isPlayer, bool moveAwayFromShooter)
    {
        bool hasSpawned = false;
        for (int i = 0; i < projectilePool.Count; i++)
        {
            if (projectilePool[i].activeSelf == false)
            {
                SetUpBullet(projectilePool[i], position, shooter, isPlayer, moveAwayFromShooter);
                hasSpawned = true;
                break;
            }
        }

        if (!hasSpawned)
        {
            int currentCount = projectilePool.Count;
            for (int i = 0; i < 5; i++)
            {
                int index = currentCount + i;
                projectilePool.Add(Instantiate(normalProjectile, poolParent.transform.position, normalProjectile.transform.rotation, poolParent.transform));
                projectilePool[index].SetActive(false);
                projectilePool[index].GetComponent<Bullet>().poolIndex = index;
                projectilePool[index].name = "Bullet: " + index;
                if (i == 0)
                {
                    SetUpBullet(projectilePool[currentCount + i], position, shooter, isPlayer, moveAwayFromShooter);
                }
            }
        }

    }
}
