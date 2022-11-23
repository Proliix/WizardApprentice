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

    float currentHealthRemoveSpeed;

    float healthbarValue;
    bool canBeHit = true;
    private void Start()
    {
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

    void SetDead()
    {
        Debug.Log(gameObject.name + " Is Dead");
    }

    IEnumerator Invicible()
    {
        canBeHit = false;
        yield return new WaitForSeconds(hitCooldown);
        canBeHit = true;
    }
}
