using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]float timeToActivate = 0.5f;
    public List<GameObject> enemyObjects;
    public bool enemiesActive { get; private set; } = false;



    public GameObject GetClosestEnemy(Vector3 point)
    {
        int closestIndex = 0;
        float closestDistance = 99999;
        for(int i = 0; i < enemyObjects.Count; i++)
        {
            if (Vector3.Distance(enemyObjects[i].transform.position,point) < closestDistance)
            {
                closestDistance = Vector3.Distance(enemyObjects[i].transform.position, point);
                closestIndex = i;
            }
        }
        if(enemyObjects.Count > 0)
        {
            return enemyObjects[closestIndex];
        }
        else
        {
            return null;
        }
    }

    public void ResetEnemyStatus()
    {
        StopAllCoroutines();
        enemiesActive = false;
    }

    public void ActivateEnemiesAfterTime()
    {
        StartCoroutine(ActivateEnemies());
    }

    IEnumerator ActivateEnemies()
    {
        yield return new WaitForSeconds(timeToActivate);
        enemiesActive = true;
    }

    public List<GameObject> GetClosestEnemy(Vector3 point, int amountToGet)
    {
        float closestDistance = 99999;
        List<int> distances = new List<int>();
        for (int i = 0; i < enemyObjects.Count; i++)
        {
            if (Vector3.Distance(enemyObjects[i].transform.position, point) < closestDistance)
            {
                distances.Add(Mathf.RoundToInt(Vector3.Distance(enemyObjects[i].transform.position, point)*1000)*1000 + i);
            }
        }
        distances.Sort();
        List<GameObject> objectToReturn = new List<GameObject>();
        for(int i = 0; i < Mathf.Min(amountToGet,enemyObjects.Count); i++)
        {
            objectToReturn.Add(enemyObjects[distances[i] % 1000]);
        }
        return objectToReturn;
    }

    public List<GameObject> GetEnemiesWithinRange(Vector3 point, float range)
    {
        List<GameObject> objectToReturn = new List<GameObject>();
        for (int i = 0; i < enemyObjects.Count; i++)
        {
            if (Vector3.Distance(enemyObjects[i].transform.position, point) < range)
            {
                objectToReturn.Add(enemyObjects[i]);
            }
        }
        return objectToReturn;
    }

    public GameObject GetEnemyWithLowestHealth()
    {
        int lowestIndex = 0;
        float lowestHealth = 99999;
        for (int i = 0; i < enemyObjects.Count; i++)
        {
            if (enemyObjects[i].GetComponent<Health>().GetHP() < lowestHealth)
            {
                lowestHealth = enemyObjects[i].GetComponent<Health>().GetHP();
                lowestIndex = i;
            }
        }
        if(enemyObjects.Count > 0)
        {
            return enemyObjects[lowestIndex];
        }
        else
        {
            return null;
        }
    }

    public GameObject GetEnemyWithHighestHealth()
    {
        int highestIndex = 0;
        float highestHealth = 99999;
        for (int i = 0; i < enemyObjects.Count; i++)
        {
            if (enemyObjects[i].GetComponent<Health>().GetHP() > highestHealth)
            {
                highestHealth = enemyObjects[i].GetComponent<Health>().GetHP();
                highestIndex = i;
            }
        }
        if(enemyObjects.Count > 0)
        {
            return enemyObjects[highestIndex];
        }
        else
        {
            return null;
        }
    }
}
