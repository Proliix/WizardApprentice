using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRendererCheck : MonoBehaviour
{

    SpriteRenderer[] spriteRenderers;
  

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            CheckSpritesInScene();
        }
    }

    private void CheckSpritesInScene()
    {
        // Get all the sprite renderers in the scene

        SpriteRenderer[] spriteRenderers = FindObjectsOfType<SpriteRenderer>();
        // Iterate through all the sprite renderers and log their information
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            Debug.Log("Sprite Renderer: " + spriteRenderer.name);
            Debug.Log(" - Sorting Order: " + spriteRenderer.sortingOrder);
        }
    }

}
