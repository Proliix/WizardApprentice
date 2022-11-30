using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RoomController : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI bossTaunt;
    [SerializeField] float tauntActiveTime;

    void Start()
    {
        bossTaunt.enabled = true;
        StartCoroutine(DestroyTaunt());
    }

   

    IEnumerator DestroyTaunt()
    {
        yield return new WaitForSeconds(tauntActiveTime);

        Destroy(bossTaunt);

        yield return null;
    }
}
