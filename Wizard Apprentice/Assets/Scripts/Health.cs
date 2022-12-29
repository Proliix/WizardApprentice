using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class Health : MonoBehaviour
{
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip deathSound;

    public delegate void DeathDelegate(GameObject gameObject);
    public event DeathDelegate deathEvent;

    [SerializeField] float maxHP = 100;
    [SerializeField] float hp = 100;
    [SerializeField] float hitCooldown = 1f;

    [SerializeField] bool removeSelf = true;
    [SerializeField] bool hasHitCooldown = false;
    [Header("UI")]
    [SerializeField] bool usesHealthBar = false;
    [SerializeField] float healthRemoveSpeed = 0.5f;
    [SerializeField] Image healthbar;
    [SerializeField] Image damageBufferhbar;
    [SerializeField] float damagebufferUpTime = 0.26f;
    [SerializeField] float damageBufferRemoveSpeed = 1f;
    [Tooltip("Removes healthbar when health is <= 0")]
    [SerializeField] bool removeHealthbar = false;
    [Header("only needed if remove healthbar is active")]
    [SerializeField] GameObject healthbarParent;
    [Header("Player Specific")]
    [SerializeField] StatsUI statsUI;
    [Header("Juice")]
    [SerializeField] bool hasDeathAnimation = false;
    [SerializeField] float hitEffectTime = 0.5f;
    [SerializeField] float hitTransparancy = 0.5f;
    [SerializeField] float flashSpeed = 0.05f;
    [SerializeField] float playerScreenShakeAmount = 1f;


    Color hitColor = new Color(0.75f, 0.5f, 0.5f, 0.5f);

    float currentHealthRemoveSpeed;

    //These quaternions is r = x g = y b = z a = w
    Quaternion currentRGBASpeed;
    Quaternion tempRGBA;
    Quaternion targetRGBA;

    Color startColor;
    Color tempColor;
    bool hitEffectActve = false;

    PlayerStats stats;
    float startHealth;
    bool isPlayer;

    HitNumberHandler hitNumbers;

    float DamageBufferValue;

    float timer = 0;
    float prevHp;
    float prevHitHp;

    SpriteRenderer spriteRenderer;
    Animator anim;
    float healthbarValue = 100f;
    bool canBeHit = true;

    private void Start()
    {
        prevHp = hp;
        prevHitHp = hp;
        if (usesHealthBar && damageBufferhbar == null)
        {
            damageBufferhbar = Instantiate(healthbar, healthbar.transform.position, healthbar.transform.rotation, healthbar.transform.parent);
            healthbar.transform.parent = damageBufferhbar.transform;
        }

        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        if (gameObject.CompareTag("Player"))
        {
            startHealth = maxHP;
            isPlayer = true;
        }

        if (hasDeathAnimation)
            anim = gameObject.GetComponent<Animator>();

        hp = maxHP;
        hitNumbers = GameObject.FindWithTag("GameController").GetComponent<HitNumberHandler>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
        tempColor = startColor;
        targetRGBA = new Quaternion(hitColor.r, hitColor.g, hitColor.b, hitTransparancy);

        if (usesHealthBar)
        {
            healthbar.gameObject.SetActive(true);
            healthbarValue = hp;
            healthbar.fillAmount = 1;
            DamageBufferValue = hp;
            damageBufferhbar.fillAmount = 1;
        }
    }

    private void Update()
    {
        if (isPlayer)
        {
            float newHp = startHealth * stats.health;
            if (maxHP != newHp)
            {
                float healHp = newHp - maxHP;
                maxHP = newHp;
                AddHealth(healHp);
            }
        }

        if (usesHealthBar)
        {
            timer += Time.deltaTime;

            if (healthbar)
            {
                if (healthbarValue == hp)
                    prevHp = hp;

                if (DamageBufferValue == hp)
                    prevHitHp = hp;

                float hpChange = prevHp - hp;
                float speedMultiplier = 1 + (Mathf.Abs(hpChange) * 0.1f);
                healthbarValue = Mathf.MoveTowards(healthbarValue, hp, healthRemoveSpeed * speedMultiplier);
                healthbar.fillAmount = (healthbarValue / maxHP);

                if (timer > damagebufferUpTime)
                {

                    hpChange = prevHitHp - hp;
                    speedMultiplier = 1 + Mathf.Abs(hpChange) * 0.1f;
                    DamageBufferValue = Mathf.MoveTowards(DamageBufferValue, hp, damageBufferRemoveSpeed * speedMultiplier);
                    damageBufferhbar.fillAmount = (DamageBufferValue / maxHP);
                }
            }
        }

        if (removeHealthbar)
            if (healthbarValue <= 0 && DamageBufferValue <= 0)
            {
                healthbarParent?.SetActive(false);
                removeHealthbar = false;
            }

        if (hitEffectActve)
        {

            UpdateColor();

            if (tempRGBA.w <= hitTransparancy + 0.05f)
            {
                targetRGBA = new Quaternion(startColor.r, startColor.g, startColor.b, startColor.a);
            }
            else if (tempRGBA.w >= 1f - 0.05f)
            {
                targetRGBA = new Quaternion(hitColor.r, hitColor.g, hitColor.b, hitTransparancy);
            }

        }
        else if (spriteRenderer.color != startColor && !hitEffectActve)
        {
            UpdateColor();
            targetRGBA = new Quaternion(startColor.r, startColor.g, startColor.b, startColor.a);
        }
    }

    void UpdateColor()
    {
        tempRGBA.x = Mathf.SmoothDamp(tempRGBA.x, targetRGBA.x, ref currentRGBASpeed.x, flashSpeed, 100, Time.deltaTime);
        tempRGBA.y = Mathf.SmoothDamp(tempRGBA.y, targetRGBA.y, ref currentRGBASpeed.y, flashSpeed, 100, Time.deltaTime);
        tempRGBA.z = Mathf.SmoothDamp(tempRGBA.z, targetRGBA.z, ref currentRGBASpeed.z, flashSpeed, 100, Time.deltaTime);
        tempRGBA.w = Mathf.SmoothDamp(tempRGBA.w, targetRGBA.w, ref currentRGBASpeed.w, flashSpeed, 100, Time.deltaTime);
        tempColor = new Color(tempRGBA.x, tempRGBA.y, tempRGBA.z, tempRGBA.w);
        spriteRenderer.color = tempColor;
    }

    public void AddHealth(float healAmount)
    {

        if ((hp + healAmount) > maxHP)
        {
            hp = maxHP;
        }
        else
        {
            hp += healAmount;

        }

        if (isPlayer)
            statsUI?.UpdateHP();

        if (hitNumbers != null)
            hitNumbers.GetHitText(transform.position, -healAmount);
    }

    public void AddMaxHealth(float healAmount)
    {

        maxHP += healAmount;
        AddHealth(healAmount);
    }
    public void AddMaxHealth(float healAmount, bool usePercent)
    {
        float healthChange = 0;
        if (usePercent)
        {
            healthChange = Mathf.Floor(GetMaxHP() * healAmount);
        }
        maxHP += healthChange;
        AddHealth(healthChange);
    }

    public bool HasFullHealth()
    {
        bool returnValue = false;

        if (hp >= maxHP)
        {
            returnValue = true;
        }

        if (isPlayer)
            statsUI?.UpdateHP();

        return returnValue;
    }

    public void FullHeal()
    {
        hp = maxHP;

        if (isPlayer)
            statsUI?.UpdateHP();

        hitNumbers?.GetHitText(transform.position, -maxHP);
    }

    /// <summary>
    /// Heals the target by maxHp / divide
    /// </summary>
    public void HealPercentageOf(float divide)
    {
        float healAmount = 0;
        healAmount = maxHP / 3;

        if (hp + healAmount <= maxHP)
            hp += healAmount;
        else
            hp = maxHP;

        if (isPlayer)
            statsUI?.UpdateHP();

        hitNumbers?.GetHitText(transform.position, -healAmount);
    }

    public void RemoveHealth(float value = 10)
    {
        if (canBeHit)
        {
            bool isCrit = false;
            if (gameObject.CompareTag("Player"))
            {
                Camera.main.GetComponent<CameraMovement>()?.GetScreenShake(hitCooldown, playerScreenShakeAmount);
            }
            else
            {
                float critDamage = stats.GetCrit(value);
                if (critDamage != value)
                {
                    isCrit = true;
                    value = critDamage;
                }
            }

            if (usesHealthBar)
                timer = 0;

            hp -= value;
            if (hp > 0)
            {
                if (hasHitCooldown)
                    SetInvicible(hitCooldown);

                StartCoroutine(Hiteffect());
            }
            else
                SetDead();

            if (isPlayer)
                statsUI?.UpdateHP();

            if (hitNumbers != null)
                hitNumbers.GetHitText(transform.position, value, isCrit);
        }
    }

    public float GetHP()
    {
        return hp;
    }

    public float GetStartMaxHp()
    {
        return startHealth;
    }

    public bool GetCanBeHit()
    {
        return canBeHit;
    }

    public float GetMaxHP()
    {
        return maxHP;
    }

    public bool GetIsDead()
    {
        bool deadState = false;

        if (hp <= 0)
            deadState = true;

        return deadState;
    }

    void SetDead()
    {
        hp = 0;

        SoundManager.Instance.PlayAudio(deathSound);

        if (gameObject.CompareTag("Player"))
        {
            if (hasDeathAnimation)
                anim.SetBool("IsDead", true);

            FindObjectOfType<DeathScreenController>().PlayerDeath();
        }
        else
        {
            //if (removeHealthbar)
            //    healthbarParent?.SetActive(false);

            if (removeSelf)
                Destroy(gameObject);
            else
                canBeHit = false;

            if (usesHealthBar == true)
            {
                healthbar.gameObject.SetActive(false);
            }
            Debug.Log("about to call event");
            if (deathEvent != null)
            {
                Debug.Log("event is not null");
                deathEvent(this.gameObject);
            }
            GameObject.FindWithTag("GameController").GetComponent<RoomManager>().RemoveEnemy(this.gameObject);
        }
    }

    public void SetInvicible(float time)
    {
        StartCoroutine(Invicible(time));
    }

    IEnumerator Hiteffect()
    {
        SoundManager.Instance.PlayAudio(hitSound);

        targetRGBA.w = hitTransparancy;
        hitEffectActve = true;
        yield return new WaitForSeconds(hitEffectTime);
        hitEffectActve = false;
    }
    IEnumerator Invicible(float cooldown)
    {
        canBeHit = false;
        yield return new WaitForSeconds(cooldown);
        canBeHit = true;
    }
}
