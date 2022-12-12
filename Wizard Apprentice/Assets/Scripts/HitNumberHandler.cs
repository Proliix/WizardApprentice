using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HitNumberHandler : MonoBehaviour
{
    [SerializeField] GameObject hitNumberHolderPrefab;
    [SerializeField] GameObject worldCanvas;
    [SerializeField] Color damageColor = new Color(1, 0.43f, 0.43f, 1);
    [SerializeField] Color healColor = new Color(0.47f, 1, 0.43f, 1);
    [SerializeField] Color critColor = new Color(0.63f, 0.12f, 0.95f, 1);
    [SerializeField] int startAmount = 25;
    [SerializeField] float upTime = 1.25f;
    [SerializeField] Vector2 offsetMax = new Vector2(0.25f, 0.25f);
    [SerializeField] Vector2 offsetMin = new Vector2(0, 0);
    [SerializeField] float critSize = 2.5f;

    float startSize;
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
        startSize = hitNumberHolderPrefab.transform.localScale.x;
    }
    IEnumerator ReturnAfterTime(float time, GameObject obj)
    {
        obj.transform.SetParent(worldCanvas.transform);
        obj.SetActive(true);
        obj.GetComponent<Animator>().SetFloat("AnimSpeed", 1 / time);
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
        obj.transform.SetParent(hitNumberPoolParent.transform);
        obj.transform.position = hitNumberPoolParent.transform.position;
    }

    GameObject SetUpText(int index, Vector3 position, float damage, bool isCrit)
    {

        GameObject newHitText = hitNumberPool[index].gameObject;
        newHitText.transform.SetParent(null);
        newHitText.transform.localPosition = position + new Vector3(Random.Range(offsetMin.x, offsetMax.x), Random.Range(offsetMin.y, offsetMax.y), 0);

        if (isCrit == false)
        {
            hitnumberText[index].color = damage > 0 ? damageColor : healColor;
            hitnumberText[index].transform.parent.transform.localScale = Vector3.one * startSize;
        }
        else
        {
            hitnumberText[index].color = critColor;
            hitnumberText[index].transform.parent.transform.localScale = Vector3.one * critSize;
        }

        hitnumberText[index].text = "" + (damage > 0 ? damage : -damage);
        return newHitText;
    }

    public GameObject GetHitText(Vector3 position, float damage, bool isCrit = false)
    {
        GameObject newHitText = hitNumberPool[0].gameObject;
        bool hasSpawned = false;

        for (int i = 0; i < hitNumberPool.Count; i++)
        {
            if (hitNumberPool[i].gameObject.activeSelf == false)
            {
                newHitText = SetUpText(i, position, damage, isCrit);
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
                    newHitText = SetUpText(i, position, damage, isCrit);
                }

            }
        }
        StartCoroutine(ReturnAfterTime(upTime, newHitText));
        return newHitText;
    }
}


