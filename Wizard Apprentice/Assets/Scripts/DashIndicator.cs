using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashIndicator : MonoBehaviour
{
 
   private bool toggleBool = true;
    Image dashImage;

    private void Start()
    {
        dashImage = gameObject.GetComponent<Image>();
    }

    public void ChangeDashIndicator()
    {
        //  toggleBool = !toggleBool;
        //  gameObject.SetActive(toggleBool);
        FillDashIcon();
    }

    private void FillDashIcon()
    {
        StartCoroutine(ImageFill.FillImageOverTime(dashImage, Color.white, 2f));
    }


}
