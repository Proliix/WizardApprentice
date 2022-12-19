using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ImageFill 
{

    public static IEnumerator FillImageOverTime(Image image, Color color, float time)
    {

        float elapsedTime = 0;
        Color originalColor = image.color;
        while (elapsedTime < time)
        {
            image.color = Color.Lerp(originalColor, color, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        image.color = color;
    }
}
   

