using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class DodgeObstacle : MonoBehaviour
{

    GameObject playerTarget;
    PlayerMovement playerMovement;
    [SerializeField] float TimesFailedDodgeroll = 0;
    float counter;
    [SerializeField] TextMeshPro textMeshPro;

    private void Start()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player");
        playerMovement = playerTarget.GetComponent<PlayerMovement>();
        TimesFailedDodgeroll = 0;
        textMeshPro.enabled = false;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            if (playerMovement.isDashing == false)
            {
            StartCoroutine(DeathByLava());
                UpdateText();
            }
            
        }
    }

    private void UpdateText()
    {
         
        if (TimesFailedDodgeroll >= 3)
        {
            textMeshPro.enabled = true;

        }

        if (TimesFailedDodgeroll >= 4 && counter < TimesFailedDodgeroll)
        {
            if (counter == 0)
                counter = TimesFailedDodgeroll + 1;
            else
                counter ++;

            textMeshPro.text += "!";
            textMeshPro.fontSize += TimesFailedDodgeroll / 50;

            if (TimesFailedDodgeroll >= 10)
            {
                textMeshPro.color = Color.red;
            }
        }
    }

    IEnumerator DeathByLava()
    {
        Vector2 Pos = playerTarget.transform.position - gameObject.transform.position;

        playerTarget.GetComponent<Health>().RemoveHealth(5);

        TimesFailedDodgeroll++;
        playerTarget.GetComponent<PlayerMovement>().SetCanMove(false);
        playerTarget.GetComponent<Rigidbody2D>().velocity = new Vector2(0, Pos.normalized.y * 5);
        yield return new WaitForSeconds(0.5f);
        playerTarget.GetComponent<PlayerMovement>().SetCanMove(true);


        yield return null;
    }

   
}

