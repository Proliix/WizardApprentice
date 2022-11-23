using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHP = 3;
    [SerializeField] int hp = 3;
    [SerializeField] float hitCooldown = 1f;


    bool canBeHit = true;

    public void AddHealth(int healAmmount)
    {
        if((hp + healAmmount) > maxHP)
        {
            hp = maxHP;
        }
        else
        {
            hp += healAmmount;
        }
    }

    public void RemoveHealth()
    {
        if (canBeHit)
        {
            hp--;
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
