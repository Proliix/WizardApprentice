using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHP = 100;
    [SerializeField] float hp = 100;
    [SerializeField] float hitCooldown = 1f;
    [Header("UI")]
    [SerializeField] float healthRemoveSpeed = 0.005f;
    [SerializeField] bool usesHealthBar = false;
    [SerializeField] Slider healthbar;
    [Header("Juice")]
    [SerializeField] float hitTransparancy = 0.5f;
    [SerializeField] float flashSpeed = 0.05f;

    float currentHealthRemoveSpeed;
    float currentAlphaSpeed;
    float tempAlpha = 1f;
    float targetAlpha = 1f;
    Color startColor;
    Color tempColor;

    SpriteRenderer spriteRenderer;
    float healthbarValue;
    bool canBeHit = true;
    private void Start()
    {
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

        if (!canBeHit)
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
        else if (spriteRenderer.color.a != 1 && canBeHit)
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
                StartCoroutine(Invicible());
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
            Destroy(gameObject);
    }

    IEnumerator Invicible()
    {
        targetAlpha = hitTransparancy;
        canBeHit = false;
        yield return new WaitForSeconds(hitCooldown);
        canBeHit = true;
    }
}
