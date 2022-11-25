using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    [Header("Normal Bullets")]
    [SerializeField] GameObject normalProjectile;
    [SerializeField] int startSizeNormal = 25;
    [Header("Special Bullets")]
    [SerializeField] GameObject specialProjectile;
    [SerializeField] int startSizeSpecial = 10;

    List<GameObject> projectilePool;
    List<GameObject> specialProjectilePool;

    GameObject poolParentNormal;
    GameObject poolParentSpecial;



    // Start is called before the first frame update
    void Start()
    {
        projectilePool = new List<GameObject>();
        specialProjectilePool = new List<GameObject>();

        poolParentNormal = new GameObject();
        poolParentNormal.name = "Normal Projectile Pool";

        poolParentSpecial = new GameObject();
        poolParentSpecial.name = "Special Projectile Pool";

        for (int i = 0; i < startSizeNormal; i++)
        {
            projectilePool.Add(Instantiate(normalProjectile, poolParentNormal.transform.position, normalProjectile.transform.rotation, poolParentNormal.transform));
            projectilePool[i].SetActive(false);
            projectilePool[i].GetComponent<Bullet>().poolIndex = i;
            projectilePool[i].name = "Bullet: " + i;
        }

        for (int i = 0; i < startSizeSpecial; i++)
        {
            specialProjectilePool.Add(Instantiate(specialProjectile, poolParentSpecial.transform.position, specialProjectile.transform.rotation, poolParentSpecial.transform));
            specialProjectilePool[i].SetActive(false);
            //Add ProjectileScript
            specialProjectilePool[i].name = "Special Bullet: " + i;
        }
    }

    public void ResetBullet(int index)
    {
        projectilePool[index].SetActive(false);
        projectilePool[index].transform.position = poolParentNormal.transform.position;
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


    /// <summary>
    /// Spawns a circle of bullets around the shooter that has Ammount of bullets.
    /// </summary>
    public void GetCircleShot(int ammount, GameObject shooter, bool isPlayer)
    {
        if (ammount <= 0)
            Debug.LogError("INVALID SIZE IN CIRCLESHOTFUNCTION MADE BY " + shooter.name);
        else
        {
            for (float deg = 0; deg < 360; deg += 360f / ammount)
            {
                float vertical = Mathf.Sin(Mathf.Deg2Rad * (deg + 90));
                float horizontal = Mathf.Cos(Mathf.Deg2Rad * (deg + 90));

                Vector3 spawnDir = new Vector3(horizontal, vertical, 0);

                Vector3 spawnPos = shooter.transform.position + spawnDir;

                GetBullet(spawnPos, shooter, isPlayer, true);
            }
        }
    }

    /// <summary>
    /// Spawns a bullet at positions. If move away form shooter is true it will change direction depending on where it spawned compared to the shooter
    /// </summary>
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
                projectilePool.Add(Instantiate(normalProjectile, poolParentNormal.transform.position, normalProjectile.transform.rotation, poolParentNormal.transform));
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
