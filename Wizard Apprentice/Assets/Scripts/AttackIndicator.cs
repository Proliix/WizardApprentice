using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackIndicator : MonoBehaviour
{
    public static AttackIndicator instance;
    public static Sprite squareSprite;
    public static Sprite circleSprite;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static void CreateSquare(Vector2 startPosition, Vector2 targetPosition, Vector2 scale, float timeAlive, bool createExpandingSquare)
    {
        GameObject indicator = new GameObject("Attack indicator");
        SpriteRenderer renderer = indicator.AddComponent<SpriteRenderer>();
        Vector2 playerDir = (targetPosition - startPosition).normalized;
        renderer.sprite = squareSprite;
        renderer.color = new Color(1,0,0,0.5f);
        indicator.transform.position = startPosition + (playerDir * scale.y * 0.5f);
        indicator.transform.localScale = scale;
        float theta = Mathf.Atan2(targetPosition.y - startPosition.y, startPosition.x - targetPosition.x);
        if (theta < 0.0)
            theta += Mathf.PI * 2;
        indicator.transform.localRotation = Quaternion.Euler(0, 0, (Mathf.Rad2Deg * theta - 90) * -1);
        if (createExpandingSquare)
        {
            instance.StartCoroutine(ExpandSquare(startPosition, targetPosition, scale, timeAlive));
        }
        Destroy(indicator, timeAlive);
    }

    public static void CreateCircle(Vector2 startPosition, float radius, float timeUntilFull, bool createExpandingCircle)
    {
        GameObject indicator = new GameObject("Attack indicator");
        SpriteRenderer renderer = indicator.AddComponent<SpriteRenderer>();
        renderer.sprite = circleSprite;
        renderer.color = new Color(1,0,0,0.5f);
        indicator.transform.position = startPosition;
        indicator.transform.localScale = new Vector2(radius,radius);
        if(createExpandingCircle)
        {
            instance.StartCoroutine(ExpandCircle(startPosition,radius,timeUntilFull));
        }
        Destroy(indicator, timeUntilFull);
    }

    public static IEnumerator ExpandSquare(Vector2 startPosition, Vector2 targetPosition, Vector2 scale, float timeUntilFull)
    {
        float currentTime = 0;
        Vector2 playerDir = (targetPosition - startPosition).normalized;
        GameObject indicator = new GameObject("Attack Indicator expanding");
        SpriteRenderer renderer = indicator.AddComponent<SpriteRenderer>();
        renderer.sprite = squareSprite;
        renderer.color = new Color(1, 0, 0, 0.7f);
        indicator.transform.position = startPosition + (playerDir * scale.y * 0.5f);
        float theta = Mathf.Atan2(targetPosition.y - startPosition.y, startPosition.x - targetPosition.x);
        if (theta < 0.0)
            theta += Mathf.PI * 2;
        indicator.transform.localRotation = Quaternion.Euler(0, 0, (Mathf.Rad2Deg * theta - 90) * -1);
        while (currentTime < timeUntilFull)
        {
            yield return null;
            currentTime += Time.deltaTime;
            indicator.transform.position = startPosition + (0.5f * (playerDir * (currentTime / timeUntilFull)) * scale.y);
            indicator.transform.localScale = new Vector2(scale.x, (currentTime / timeUntilFull) * scale.y);
        }
        Destroy(indicator);
    }

    public static IEnumerator ExpandCircle(Vector2 startPosition, float radius, float timeUntilFull)
    {
        float currentTime = 0;
        GameObject indicator = new GameObject("Attack Indicator expanding");
        SpriteRenderer renderer = indicator.AddComponent<SpriteRenderer>();
        renderer.sprite = circleSprite;
        renderer.color = new Color(1, 0, 0, 0.7f);
        indicator.transform.position = startPosition;
        while (currentTime < timeUntilFull)
        {
            yield return null;
            currentTime += Time.deltaTime;
            indicator.transform.localScale = new Vector2((currentTime / timeUntilFull)* radius, (currentTime / timeUntilFull) * radius);
        }
        Destroy(indicator);
    }
}
