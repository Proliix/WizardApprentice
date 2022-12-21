using UnityEngine;
using UnityEngine.UI;

public class RoomFadeIn : MonoBehaviour
{
    public float fadeTime = 1.0f; // duration of the fade-out effect
    static private float currentTime = 1.0f; // elapsed time since the fade-out started
    static private Image image; // reference to the UI Image component

  
 

    
    void Awake()
    {
        // get a reference to the UI Image component
        image = GetComponent<Image>();
       
    }

    void Update()
    {
        // decrement the current time
        currentTime -= Time.deltaTime;

        // if the current time is greater than or equal to 0, calculate the alpha value for the image
        if (currentTime >= 0)
        {
            float alpha = currentTime / fadeTime;
            image.color = new Color(0.0f, 0.0f, 0.0f, alpha);
        }
        // if the current time is less than 0, set the alpha value to 0.0
        else
        {
            image.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            NewRoom();
        }
    }

   static public void NewRoom()
    {

        currentTime = 0.5f;
        // set the initial color of the image to fully opaque
        image.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
    }
}
