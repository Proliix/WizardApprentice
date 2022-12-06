using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HitNumberHandler : MonoBehaviour
{
    [SerializeField] GameObject hitNumberHolderPrefab;
    [SerializeField] GameObject worldCanvas;
    [SerializeField] int startAmount = 25;

    GameObject hitNumberPoolParent;
    List<GameObject> hitNumberPool;
    List<TextMeshProUGUI> hitnumberText;


    // Start is called before the first frame update
    void Start()
    {
        hitnumberText = new List<TextMeshProUGUI>();
        hitNumberPool = new List<GameObject>();

        hitNumberPoolParent = new GameObject("Hit Number Pool");
        hitNumberPoolParent.transform.position = Vector3.one * 35;
        for (int i = 0; i < startAmount; i++)
        {
            hitNumberPool.Add(Instantiate(hitNumberHolderPrefab, hitNumberPoolParent.transform.position, hitNumberHolderPrefab.transform.rotation, hitNumberPoolParent.transform));
            hitnumberText.Add(hitNumberPool[i].GetComponentInChildren<TextMeshProUGUI>());
            hitNumberPool[i].gameObject.SetActive(false);
            hitNumberPool[i].gameObject.name = "Hit number text " + i;
        }
    }
    IEnumerator ReturnAfterTime(float time, GameObject obj)
    {
        obj.transform.SetParent(worldCanvas.transform);
        obj.SetActive(true);
        obj.GetComponent<Animator>().SetFloat("AnimSpeed", time);
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
        obj.transform.SetParent(hitNumberPoolParent.transform);
        obj.transform.position = hitNumberPoolParent.transform.position;
    }

    GameObject SetUpText(int index, Vector3 position, float damage)
    {

        GameObject newHitText = hitNumberPool[index].gameObject;
        newHitText.transform.SetParent(null);
        newHitText.transform.localPosition = position;
        hitnumberText[index].text = "" + damage;
        return newHitText;
    }

    public GameObject GetHitText(Vector3 position, float damage, float uptime = 1)
    {
        GameObject newHitText = hitNumberPool[0].gameObject;
        bool hasSpawned = false;

        for (int i = 0; i < hitNumberPool.Count; i++)
        {
            if (hitNumberPool[i].gameObject.activeSelf == false)
            {
                newHitText = SetUpText(i, position, damage);
                hasSpawned = true;
                break;
            }
        }

        if (!hasSpawned)
        {
            int currentCount = hitNumberPool.Count;
            for (int i = 0; i < 5; i++)
            {
                int index = currentCount + i;
                hitNumberPool.Add(Instantiate(hitNumberHolderPrefab, hitNumberPoolParent.transform.position, hitNumberHolderPrefab.transform.rotation, hitNumberPoolParent.transform));
                hitnumberText.Add(hitNumberPool[i].GetComponentInChildren<TextMeshProUGUI>());
                hitNumberPool[index].gameObject.SetActive(false);
                hitNumberPool[index].gameObject.name = "Hit number text " + index;
                if (i == 0)
                {
                    newHitText = SetUpText(i, position, damage);
                }

            }
        }
        StartCoroutine(ReturnAfterTime(uptime, newHitText));
        return newHitText;
    }
}


