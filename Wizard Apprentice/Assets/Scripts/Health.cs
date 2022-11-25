using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHP = 100;
    [SerializeField] float hp = 100;
    [SerializeField] float hitCooldown = 1f;
    [SerializeField] bool hasHitCooldown = false;
    [Header("UI")]
    [SerializeField] float healthRemoveSpeed = 0.005f;
    [SerializeField] bool usesHealthBar = false;
    [SerializeField] Slider healthbar;
    [Header("Juice")]
    [SerializeField] float hitEffectTime = 0.5f;
    [SerializeField] float hitTransparancy = 0.5f;
    [SerializeField] float flashSpeed = 0.05f;

    float currentHealthRemoveSpeed;
    float currentAlphaSpeed;
    float tempAlpha = 1f;
    float targetAlpha = 1f;
    Color startColor;
    Color tempColor;
    bool hitEffectActve = false;


    SpriteRenderer spriteRenderer;
    float healthbarValue;
    bool canBeHit = true;
    private void Start()
    {
        hp = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
        tempColor = startColor;
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
            tempAlpha = Mathf.SmoothDamp(tempAlpha, targetAlpha, ref currentAlphaSpeed, flashSpeed, 100, Time.deltaTime);
            tempColor.a = tempAlpha;
            spriteRenderer.color = tempColor;

            if (tempAlpha <= hitTransparancy + 0.05f)
            {
                targetAlpha = 1f;
            }
            else if (tempAlpha >= 1f - 0.05f)
            {
                targetAlpha = hitTransparancy;
            }

        }
        else if (spriteRenderer.color.a != 1 && !hitEffectActve)
        {
            tempAlpha = Mathf.SmoothDamp(tempAlpha, 1, ref currentAlphaSpeed, flashSpeed, 100, Time.deltaTime);
            tempColor.a = tempAlpha;
            spriteRenderer.color = tempColor;
        }
    }

    public void AddHealth(int healAmmount)
    {
        if ((hp + healAmmount) > maxHP)
        {
            hp = maxHP;
        }
        else
        {
            hp += healAmmount;
        }

    }

    public void RemoveHealth(float value = 10)
    {
        if (canBeHit)
        {
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

    void SetDead()
    {
        hp = 0;
        if (gameObject.CompareTag("Player"))
            Debug.Log(gameObject.name + " Is Dead");
        else
        {
            Destroy(gameObject);
            GameObject.FindWithTag("GameController").GetComponent<RoomManager>().RemoveEnemy(this.gameObject);
        }
    }

    public void SetInvicible(float time)
    {
        StartCoroutine(Invicible(time));
    }

    IEnumerator Hiteffect()
    {
        targetAlpha = hitTransparancy;
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
