using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashIndicator : MonoBehaviour
{
 
   private bool toggleBool = true;

    
    public void ChangeDashIndicator()
    {
        toggleBool = !toggleBool;
        gameObject.SetActive(toggleBool);
    }
   


}
