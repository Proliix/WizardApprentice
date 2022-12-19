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
            specialProjectilePool[i].GetComponent<SpecialProjectile>().poolIndex = i;
            specialProjectilePool[i].name = "Special Bullet: " + i;
        }
    }

    public void UpdateBullet(GameObject bullet, GameObject Shooter, Vector3 dir, bool isPlayer)
    {
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.shooter = Shooter;
        bulletScript.isPlayerBullet = isPlayer;
        bulletScript.ResetTimer();
        bulletScript.UpdateColor();
        bulletScript.UpdateDirection(dir);
    }

    public void ResetBullet(int index)
    {
        projectilePool[index].SetActive(false);
        projectilePool[index].transform.position = poolParentNormal.transform.position;
    }

    public void ResetSpecialBullet(int index)
    {
        specialProjectilePool[index].SetActive(false);
        specialProjectilePool[index].transform.position = poolParentSpecial.transform.position;
    }

    public void ResetAll()
    {
        if (specialProjectilePool == null || projectilePool == null)
        {
            return;
        }

        for (int i = 0; i < specialProjectilePool.Count; i++)
        {
            if (specialProjectilePool[i].activeSelf == true)
            {
                specialProjectilePool[i].SetActive(false);
                specialProjectilePool[i].transform.position = poolParentSpecial.transform.position;
            }
        }

        for (int i = 0; i < projectilePool.Count; i++)
        {
            if (projectilePool[i].activeSelf == true)
            {
                projectilePool[i].SetActive(false);
                projectilePool[i].transform.position = poolParentNormal.transform.position;
            }
        }

    }

    void SetUpBullet(GameObject poolMember, Vector3 position, Vector3 direction, GameObject shooter, bool isPlayer, bool moveAwayFromShooter, float damage, float size, float speed)
    {
        poolMember.transform.position = position;
        poolMember.transform.localScale = Vector3.one * size;
        Bullet bullet = poolMember.GetComponent<Bullet>();
        bullet.bulletSpeed = speed;
        bullet.damage = damage;
        bullet.shooter = shooter;
        bullet.isPlayerBullet = isPlayer;
        bullet.moveAwayFromShoter = moveAwayFromShooter;
        bullet.UpdateColor();
        if (moveAwayFromShooter || direction == Vector3.zero)
            bullet.UpdateDirection();
        else
            bullet.UpdateDirection(direction);
        bullet.ResetTimer();
        poolMember.SetActive(true);
    }

    void SetUpBullet(GameObject poolMember, Vector3 position, Vector3 direction, bool isPlayer, float damage, float size, float speed)
    {
        poolMember.transform.position = position;
        poolMember.transform.localScale = Vector3.one * size;
        Bullet bullet = poolMember.GetComponent<Bullet>();
        bullet.bulletSpeed = speed;
        bullet.damage = damage;
        bullet.isPlayerBullet = isPlayer;
        bullet.UpdateColor();
        bullet.UpdateDirection(direction);
        bullet.ResetTimer();
        poolMember.SetActive(true);
    }

    void SetUpBullet(GameObject poolMember, Transform shootPos, GameObject shooter, bool isPlayer, bool moveAwayFromShooter, Vector3 posDeviation, float damage, float size, float speed)
    {


        //FIND BETTER SOLUTION FOR CORRECT POSITIONS WHEN ROTATING
        poolMember.transform.parent = shootPos.transform;
        poolMember.transform.position = shootPos.transform.position;
        poolMember.transform.rotation = shootPos.transform.rotation;
        poolMember.transform.localPosition = poolMember.transform.localPosition + posDeviation;
        poolMember.transform.parent = poolParentNormal.transform;
        //________________________________________________________


        poolMember.transform.localScale = Vector3.one * size;
        Bullet bullet = poolMember.GetComponent<Bullet>();
        bullet.bulletSpeed = speed;
        bullet.shooter = shooter;
        bullet.damage = damage;
        bullet.isPlayerBullet = isPlayer;
        bullet.moveAwayFromShoter = moveAwayFromShooter;
        bullet.UpdateColor();
        bullet.UpdateDirection();
        bullet.ResetTimer();
        poolMember.SetActive(true);
    }

    void SetupSpecialBullet(GameObject poolMember, Vector3 position, GameObject newShooter, Sprite bulletImage, SpecialBulletState newBulletState, ICard icard, bool isPlayer, float effectCooldown, float lifetime, bool moveAwayFromShooter, float damage, float size, float speed, float effectSize)
    {
        poolMember.transform.position = position;
        poolMember.GetComponent<SpriteRenderer>().sprite = bulletImage;
        poolMember.transform.localScale = Vector3.one * size;
        SpecialProjectile specialBullet = poolMember.GetComponent<SpecialProjectile>();
        specialBullet.bulletState = newBulletState;
        specialBullet.effectSize = effectSize;
        specialBullet.damage = damage;
        specialBullet.bulletSpeed = speed;
        specialBullet.Shooter = newShooter;
        specialBullet.currentIcard = icard;
        specialBullet.isMovingAway = moveAwayFromShooter;
        specialBullet.isPlayerBullet = isPlayer;
        specialBullet.effectCooldown = effectCooldown;
        specialBullet.bulletLifetime = lifetime;
        poolMember.SetActive(true);
    }

    //with direction
    void SetupSpecialBullet(GameObject poolMember, Vector3 position, Vector3 direction, GameObject newShooter, Sprite bulletImage, SpecialBulletState newBulletState, ICard icard, bool isPlayer, float effectCooldown, float lifetime, float damage, float size, float speed, float effectSize)
    {
        poolMember.transform.position = position;
        poolMember.GetComponent<SpriteRenderer>().sprite = bulletImage;
        poolMember.transform.localScale = Vector3.one * size;
        SpecialProjectile specialBullet = poolMember.GetComponent<SpecialProjectile>();
        specialBullet.bulletState = newBulletState;
        specialBullet.UpdateDirection(direction);
        specialBullet.effectSize = effectSize;
        specialBullet.damage = damage;
        specialBullet.bulletSpeed = speed;
        specialBullet.Shooter = newShooter;
        specialBullet.currentIcard = icard;
        specialBullet.isPlayerBullet = isPlayer;
        specialBullet.effectCooldown = effectCooldown;
        specialBullet.bulletLifetime = lifetime;
        poolMember.SetActive(true);
    }

    //With anim
    void SetupSpecialBullet(GameObject poolMember, Vector3 position, GameObject newShooter, Sprite bulletImage, SpecialBulletState newBulletState, ICard icard, bool isPlayer, string animatonName, float effectCooldown, float lifetime, bool moveAwayFromShooter, float damage, float size, float speed, float effectSize)
    {
        poolMember.transform.position = position;
        poolMember.GetComponent<SpriteRenderer>().sprite = bulletImage;
        poolMember.transform.localScale = Vector3.one * size;
        SpecialProjectile specialBullet = poolMember.GetComponent<SpecialProjectile>();
        specialBullet.bulletState = newBulletState;
        specialBullet.damage = damage;
        specialBullet.bulletSpeed = speed;
        specialBullet.effectSize = effectSize;
        specialBullet.Shooter = newShooter;
        specialBullet.currentIcard = icard;
        specialBullet.isMovingAway = moveAwayFromShooter;
        specialBullet.isPlayerBullet = isPlayer;
        specialBullet.effectCooldown = effectCooldown;
        specialBullet.bulletLifetime = lifetime;
        poolMember.SetActive(true);
        specialBullet.SetAnimationBool(animatonName, true);
    }
    //with anim towards mouse
    void SetupSpecialBullet(GameObject poolMember, Transform shootPos, GameObject newShooter, Sprite bulletImage, SpecialBulletState newBulletState, ICard icard, bool isPlayer, string animatonName, float effectCooldown, float lifetime, bool moveAwayFromShooter, float damage, float size, float speed, float effectSize, Vector3 posDeviation)
    {

        //FIND BETTER SOLUTION FOR CORRECT POSITIONS WHEN ROTATING
        poolMember.transform.parent = shootPos.transform;
        poolMember.transform.position = shootPos.transform.position;
        poolMember.transform.rotation = shootPos.transform.rotation;
        poolMember.transform.localPosition = poolMember.transform.localPosition + posDeviation;
        poolMember.transform.parent = poolParentSpecial.transform;
        //________________________________________________________

        poolMember.GetComponent<SpriteRenderer>().sprite = bulletImage;
        poolMember.transform.localScale = Vector3.one * size;
        SpecialProjectile specialBullet = poolMember.GetComponent<SpecialProjectile>();
        specialBullet.bulletState = newBulletState;
        specialBullet.damage = damage;
        specialBullet.bulletSpeed = speed;
        specialBullet.effectSize = effectSize;
        specialBullet.Shooter = newShooter;
        specialBullet.currentIcard = icard;
        specialBullet.isMovingAway = moveAwayFromShooter;
        specialBullet.isPlayerBullet = isPlayer;
        specialBullet.effectCooldown = effectCooldown;
        specialBullet.bulletLifetime = lifetime;
        poolMember.SetActive(true);
        specialBullet.SetAnimationBool(animatonName, true);
    }

    void SetupSpecialBullet(GameObject poolMember, Transform shootPos, GameObject newShooter, Sprite bulletImage, SpecialBulletState newBulletState, ICard icard, bool isPlayer, Vector3 posDeviation, float effectCooldown, float lifetime, bool moveAwayFromShooter, float damage, float size, float speed, float effectSize)
    {

        //FIND BETTER SOLUTION FOR CORRECT POSITIONS WHEN ROTATING
        poolMember.transform.parent = shootPos.transform;
        poolMember.transform.position = shootPos.transform.position;
        poolMember.transform.rotation = shootPos.transform.rotation;
        poolMember.transform.localPosition = poolMember.transform.localPosition + posDeviation;
        poolMember.transform.parent = poolParentSpecial.transform;
        //________________________________________________________


        poolMember.transform.localScale = Vector3.one * size;
        poolMember.GetComponent<SpriteRenderer>().sprite = bulletImage;
        SpecialProjectile specialBullet = poolMember.GetComponent<SpecialProjectile>();
        specialBullet.damage = damage;
        specialBullet.bulletSpeed = speed;
        specialBullet.bulletState = newBulletState;
        specialBullet.effectSize = effectSize;
        specialBullet.Shooter = newShooter;
        specialBullet.currentIcard = icard;
        specialBullet.isMovingAway = moveAwayFromShooter;
        specialBullet.isPlayerBullet = isPlayer;
        specialBullet.effectCooldown = effectCooldown;
        specialBullet.bulletLifetime = lifetime;
        poolMember.SetActive(true);
    }



    /// <summary>
    /// Spawns a circle of bullets around the shooter that has Amount of bullets.
    /// </summary>
    public void GetCircleShot(int amount, GameObject shooter, bool isPlayer, float damage = 10f, float bulletSize = 0.5f, float speed = 10f)
    {
        if (amount <= 0)
            Debug.LogError("INVALID SIZE IN CIRCLESHOTFUNCTION MADE BY " + shooter.name);
        else
        {
            for (float deg = 0; deg < 360; deg += 360f / amount)
            {
                float vertical = Mathf.Sin(Mathf.Deg2Rad * (deg + 90));
                float horizontal = Mathf.Cos(Mathf.Deg2Rad * (deg + 90));

                Vector3 spawnDir = new Vector3(horizontal, vertical, 0);

                Vector3 spawnPos = shooter.transform.position + spawnDir;

                GetBullet(spawnPos, shooter, isPlayer, true, damage, bulletSize, speed);
            }
        }
    }

    /// <summary>
    /// Spawns a circle of bullets around the shooter that has Amount of bullets. Changes where the bullets spawns depending on angle
    /// </summary>
    public void GetCircleShot(int amount, GameObject shooter, bool isPlayer, float angle, float damage = 10f, float bulletSize = 0.5f, float speed = 10f)
    {
        if (amount <= 0)
            Debug.LogError("INVALID SIZE IN CIRCLESHOTFUNCTION MADE BY " + shooter.name);
        else
        {
            for (float deg = 0; deg < 360; deg += 360f / amount)
            {
                float vertical = Mathf.Sin(Mathf.Deg2Rad * (deg + 90 + angle));
                float horizontal = Mathf.Cos(Mathf.Deg2Rad * (deg + 90 + angle));

                Vector3 spawnDir = new Vector3(horizontal, vertical, 0);

                Vector3 spawnPos = shooter.transform.position + spawnDir;

                GetBullet(spawnPos, shooter, isPlayer, true, damage, bulletSize, speed);
            }
        }
    }

    /// <summary>
    /// Spawns a bullet at position. If move away form shooter is true it will change direction depending on where it spawned compared to the shooter
    /// </summary>
    public void GetBullet(Vector3 position, GameObject shooter, bool isPlayer, bool moveAwayFromShooter, float damage = 10f, float size = 0.5f, float speed = 10f)
    {
        bool hasSpawned = false;
        for (int i = 0; i < projectilePool.Count; i++)
        {
            if (projectilePool[i].activeSelf == false)
            {
                SetUpBullet(projectilePool[i], position, Vector3.zero, shooter, isPlayer, moveAwayFromShooter, damage, size, speed);
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
                    SetUpBullet(projectilePool[currentCount + i], position, Vector3.zero, shooter, isPlayer, moveAwayFromShooter, damage, size, speed);
                }
            }
        }

    }

    /// <summary>
    /// Spawns a bullet at position towards direction. If move away form shooter is true it will change direction depending on where it spawned compared to the shooter
    /// </summary>
    public void GetBullet(Vector3 position, Vector3 direciton, GameObject shooter, bool isPlayer, bool moveAwayFromShooter, float damage = 10f, float size = 0.5f, float speed = 10f)
    {
        bool hasSpawned = false;
        for (int i = 0; i < projectilePool.Count; i++)
        {
            if (projectilePool[i].activeSelf == false)
            {
                SetUpBullet(projectilePool[i], position, direciton, shooter, isPlayer, moveAwayFromShooter, damage, size, speed);
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
                    SetUpBullet(projectilePool[currentCount + i], position, direciton, shooter, isPlayer, moveAwayFromShooter, damage, size, speed);
                }
            }
        }

    }

    /// <summary>
    /// Spawns a bullet at position. It will move in the given direction
    /// </summary>
    public void GetBullet(Vector3 position, Vector3 direction, bool isPlayer, float damage = 10f, float size = 0.5f, float speed = 10f)
    {
        bool hasSpawned = false;
        for (int i = 0; i < projectilePool.Count; i++)
        {
            if (projectilePool[i].activeSelf == false)
            {
                SetUpBullet(projectilePool[i], position, direction, isPlayer, damage, size, speed);
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
                    SetUpBullet(projectilePool[currentCount + i], position, direction, isPlayer, damage, size, speed);
                }
            }
        }

    }

    /// <summary>
    /// Spawns a bullet at shootpos  with shootpos's rotation. If move away form shooter is true it will change direction depending on where it spawned compared to the shooter
    /// </summary>
    public void GetBullet(Transform shootPos, GameObject shooter, bool isPlayer, bool moveAwayFromShooter, float damage = 10f, float size = 0.5f, float speed = 10f)
    {
        bool hasSpawned = false;
        for (int i = 0; i < projectilePool.Count; i++)
        {
            if (projectilePool[i].activeSelf == false)
            {
                SetUpBullet(projectilePool[i], shootPos, shooter, isPlayer, moveAwayFromShooter, Vector3.zero, damage, size, speed);
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
                    SetUpBullet(projectilePool[currentCount + i], shootPos, shooter, isPlayer, moveAwayFromShooter, Vector3.zero, damage, size, speed);
                }
            }
        }

    }
    /// <summary>
    /// Spawns a bullet at shootpos + posdeviation with shootpos's rotation. If move away form shooter is true it will change direction depending on where it spawned compared to the shooter
    /// </summary>
    public void GetBullet(Transform shootPos, GameObject shooter, bool isPlayer, bool moveAwayFromShooter, Vector3 posDeviation, float damage = 10f, float size = 0.5f, float speed = 10f)
    {
        bool hasSpawned = false;
        for (int i = 0; i < projectilePool.Count; i++)
        {
            if (projectilePool[i].activeSelf == false)
            {
                SetUpBullet(projectilePool[i], shootPos, shooter, isPlayer, moveAwayFromShooter, posDeviation, damage, size, speed);
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
                    SetUpBullet(projectilePool[currentCount + i], shootPos, shooter, isPlayer, moveAwayFromShooter, posDeviation, damage, size, speed);
                }
            }
        }

    }


    public GameObject GetSpecialBullet(Vector3 position, GameObject newShooter, Sprite bulletImage, SpecialBulletState newBulletState, ICard icard, bool isPlayer, float effectCooldown = 0, float lifeTime = 2.5f, bool moveAwayFromShooter = false, float damage = 10f, float size = 0.5f, float speed = 10f, float effectSize = 1)
    {
        bool hasSpawned = false;
        GameObject newBullet = specialProjectilePool[0];
        for (int i = 0; i < specialProjectilePool.Count; i++)
        {
            if (specialProjectilePool[i].activeSelf == false)
            {
                SetupSpecialBullet(specialProjectilePool[i], position, newShooter, bulletImage, newBulletState, icard, isPlayer, effectCooldown, lifeTime, moveAwayFromShooter, damage, size, speed, effectSize);
                newBullet = specialProjectilePool[i];
                hasSpawned = true;
                break;
            }
        }

        if (!hasSpawned)
        {
            int currentCount = specialProjectilePool.Count;
            for (int i = 0; i < 5; i++)
            {
                int index = currentCount + i;
                specialProjectilePool.Add(Instantiate(specialProjectile, poolParentSpecial.transform.position, specialProjectile.transform.rotation, poolParentSpecial.transform));
                specialProjectilePool[index].SetActive(false);
                specialProjectilePool[index].GetComponent<SpecialProjectile>().poolIndex = index;
                specialProjectilePool[index].name = "Special Bullet: " + index;
                if (i == 0)
                {
                    SetupSpecialBullet(specialProjectilePool[currentCount + i], position, newShooter, bulletImage, newBulletState, icard, isPlayer, effectCooldown, lifeTime, moveAwayFromShooter, damage, size, speed, effectSize);
                    newBullet = specialProjectilePool[currentCount + i];
                }

            }

        }

        return newBullet;
    }

    /// <summary>
    /// Shoots special bullet in direciton
    /// </summary>
    public GameObject GetSpecialBullet(Vector3 position, GameObject newShooter, Vector3 direction, Sprite bulletImage, SpecialBulletState newBulletState, ICard icard, bool isPlayer, float effectCooldown = 0, float lifeTime = 2.5f, float damage = 10f, float size = 0.5f, float speed = 10f, float effectSize = 1)
    {
        bool hasSpawned = false;
        GameObject newBullet = specialProjectilePool[0];
        for (int i = 0; i < specialProjectilePool.Count; i++)
        {
            if (specialProjectilePool[i].activeSelf == false)
            {
                SetupSpecialBullet(specialProjectilePool[i], position, direction, newShooter, bulletImage, newBulletState, icard, isPlayer, effectCooldown, lifeTime, damage, size, speed, effectSize);
                newBullet = specialProjectilePool[i];
                hasSpawned = true;
                break;
            }
        }

        if (!hasSpawned)
        {
            int currentCount = specialProjectilePool.Count;
            for (int i = 0; i < 5; i++)
            {
                int index = currentCount + i;
                specialProjectilePool.Add(Instantiate(specialProjectile, poolParentSpecial.transform.position, specialProjectile.transform.rotation, poolParentSpecial.transform));
                specialProjectilePool[index].SetActive(false);
                specialProjectilePool[index].GetComponent<SpecialProjectile>().poolIndex = index;
                specialProjectilePool[index].name = "Special Bullet: " + index;
                if (i == 0)
                {
                    SetupSpecialBullet(specialProjectilePool[currentCount + i], position, direction, newShooter, bulletImage, newBulletState, icard, isPlayer, effectCooldown, lifeTime, damage, size, speed, effectSize);
                    newBullet = specialProjectilePool[currentCount + i];
                }

            }

        }

        return newBullet;
    }


    /// <summary>
    /// Shoots special bullet towards mouse
    /// </summary>
    public GameObject GetSpecialBullet(Transform shotpos, GameObject newShooter, Sprite bulletImage, SpecialBulletState newBulletState, ICard icard, bool isPlayer, Vector3 posDeviation, float effectCooldown = 0, float lifeTime = 2.5f, bool moveAwayFromShooter = false, float damage = 10f, float size = 0.5f, float speed = 10f, float effectSize = 1)
    {
        bool hasSpawned = false;
        GameObject newBullet = specialProjectilePool[0];
        for (int i = 0; i < specialProjectilePool.Count; i++)
        {
            if (specialProjectilePool[i].activeSelf == false)
            {
                SetupSpecialBullet(specialProjectilePool[i], shotpos, newShooter, bulletImage, newBulletState, icard, isPlayer, posDeviation, effectCooldown, lifeTime, moveAwayFromShooter, damage, size, speed, effectSize);
                newBullet = specialProjectilePool[i];
                hasSpawned = true;
                break;
            }
        }

        if (!hasSpawned)
        {
            int currentCount = specialProjectilePool.Count;
            for (int i = 0; i < 5; i++)
            {
                int index = currentCount + i;
                specialProjectilePool.Add(Instantiate(specialProjectile, poolParentSpecial.transform.position, specialProjectile.transform.rotation, poolParentSpecial.transform));
                specialProjectilePool[index].SetActive(false);
                specialProjectilePool[index].GetComponent<SpecialProjectile>().poolIndex = index;
                specialProjectilePool[index].name = "Special Bullet: " + index;
                if (i == 0)
                {
                    SetupSpecialBullet(specialProjectilePool[currentCount + i], shotpos, newShooter, bulletImage, newBulletState, icard, isPlayer, posDeviation, effectCooldown, lifeTime, moveAwayFromShooter, damage, size, speed, effectSize);
                    newBullet = specialProjectilePool[currentCount + i];
                }

            }

        }

        return newBullet;
    }

    /// <summary>
    /// Shoots special bullet with animation depending on the animation name corresponding to a bool in specialbullet animationcontroller
    /// </summary>
    public GameObject GetSpecialBullet(Vector3 position, GameObject newShooter, Sprite bulletImage, SpecialBulletState newBulletState, ICard icard, bool isPlayer, string animationName, float effectCooldown = 0, float lifeTime = 2.5f, bool moveAwayFromShooter = false, float damage = 10f, float size = 0.5f, float speed = 10f, float effectSize = 1)
    {
        bool hasSpawned = false;
        GameObject newBullet = specialProjectilePool[0];
        for (int i = 0; i < specialProjectilePool.Count; i++)
        {
            if (specialProjectilePool[i].activeSelf == false)
            {
                SetupSpecialBullet(specialProjectilePool[i], position, newShooter, bulletImage, newBulletState, icard, isPlayer, animationName, effectCooldown, lifeTime, moveAwayFromShooter, damage, size, speed, effectSize);
                newBullet = specialProjectilePool[i];
                hasSpawned = true;
                break;
            }
        }

        if (!hasSpawned)
        {
            int currentCount = specialProjectilePool.Count;
            for (int i = 0; i < 5; i++)
            {
                int index = currentCount + i;
                specialProjectilePool.Add(Instantiate(specialProjectile, poolParentSpecial.transform.position, specialProjectile.transform.rotation, poolParentSpecial.transform));
                specialProjectilePool[index].SetActive(false);
                specialProjectilePool[index].GetComponent<SpecialProjectile>().poolIndex = index;
                specialProjectilePool[index].name = "Special Bullet: " + index;
                if (i == 0)
                {
                    SetupSpecialBullet(specialProjectilePool[currentCount + i], position, newShooter, bulletImage, newBulletState, icard, isPlayer, animationName, effectCooldown, lifeTime, moveAwayFromShooter, damage, size, speed, effectSize);
                    newBullet = specialProjectilePool[currentCount + i];
                }

            }

        }

        return newBullet;
    }

    /// <summary>
    /// Shoots special bullet towards mouse with animation depending on the animation name corresponding to a bool in specialbullet animationcontroller
    /// </summary>
    public GameObject GetSpecialBullet(Transform shotPos, GameObject newShooter, Sprite bulletImage, SpecialBulletState newBulletState, ICard icard, bool isPlayer, string animationName, Vector3 shotDiveation, float effectCooldown = 0, float lifeTime = 2.5f, bool moveAwayFromShooter = false, float damage = 10f, float size = 0.5f, float speed = 10f, float effectSize = 1)
    {
        bool hasSpawned = false;
        GameObject newBullet = specialProjectilePool[0];
        for (int i = 0; i < specialProjectilePool.Count; i++)
        {
            if (specialProjectilePool[i].activeSelf == false)
            {
                SetupSpecialBullet(specialProjectilePool[i], shotPos, newShooter, bulletImage, newBulletState, icard, isPlayer, animationName, effectCooldown, lifeTime, moveAwayFromShooter, damage, size, speed, effectSize, shotDiveation);
                newBullet = specialProjectilePool[i];
                hasSpawned = true;
                break;
            }
        }

        if (!hasSpawned)
        {
            int currentCount = specialProjectilePool.Count;
            for (int i = 0; i < 5; i++)
            {
                int index = currentCount + i;
                specialProjectilePool.Add(Instantiate(specialProjectile, poolParentSpecial.transform.position, specialProjectile.transform.rotation, poolParentSpecial.transform));
                specialProjectilePool[index].SetActive(false);
                specialProjectilePool[index].GetComponent<SpecialProjectile>().poolIndex = index;
                specialProjectilePool[index].name = "Special Bullet: " + index;
                if (i == 0)
                {
                    SetupSpecialBullet(specialProjectilePool[currentCount + i], shotPos, newShooter, bulletImage, newBulletState, icard, isPlayer, animationName, effectCooldown, lifeTime, moveAwayFromShooter, damage, size, speed, effectSize, shotDiveation);
                    newBullet = specialProjectilePool[currentCount + i];
                }

            }

        }

        return newBullet;
    }
}
