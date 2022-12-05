using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip deathSound;



    [SerializeField] float maxHP = 100;
    [SerializeField] float hp = 100;
    [SerializeField] float hitCooldown = 1f;

    [SerializeField] bool removeSelf = true;
    [SerializeField] bool hasHitCooldown = false;
    [Header("UI")]
    [SerializeField] float healthRemoveSpeed = 0.005f;
    [SerializeField] bool usesHealthBar = false;
    [SerializeField] Slider healthbar;
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


    SpriteRenderer spriteRenderer;
    Animator anim;
    float healthbarValue;
    bool canBeHit = true;
    private void Start()
    {
        if (hasDeathAnimation)
            anim = gameObject.GetComponent<Animator>();

        hp = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
        tempColor = startColor;
        targetRGBA = new Quaternion(hitColor.r, hitColor.g, hitColor.b, hitTransparancy);


        if (usesHealthBar)
            healthbar.gameObject.SetActive(true);

        if (usesHealthBar)
        {
            healthbar.maxValue = maxHP;
            healthbar.minValue = 0;
            healthbarValue = hp;
        }
    }

    private void Update()
    {
        if (usesHealthBar)
        {
            healthbarValue = Mathf.SmoothDamp(healthbarValue, hp, ref currentHealthRemoveSpeed, healthRemoveSpeed, 100, Time.deltaTime);
            healthbar.value = healthbarValue;
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

    }

    public void AddMaxHealth(float healAmount)
    {
        maxHP += healAmount;
        if (usesHealthBar)
            healthbar.maxValue = maxHP;
        AddHealth(healAmount);

    }

    public void RemoveHealth(float value = 10)
    {
        if (canBeHit)
        {
            if (gameObject.CompareTag("Player"))
                Camera.main.GetComponent<CameraMovement>()?.GetScreenShake(hitCooldown, playerScreenShakeAmount);

            hp -= value;
            if (hp > 0)
            {
                if (hasHitCooldown)
                    SetInvicible(hitCooldown);

                StartCoroutine(Hiteffect());
            }
            else
                SetDead();
        }
    }

    public float GetHP()
    {
        return hp;
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
            if (removeSelf)
                Destroy(gameObject);

            if (usesHealthBar == true)
            {
                healthbar.gameObject.SetActive(false);
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
