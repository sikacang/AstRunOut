using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public GameObject pool;
    public int jumlahPool;
    
    public Transform platformsParent;

    private List<GameObject> objectPools;
    private void Start()
    {
        objectPools = new List<GameObject>();

        for (int i = 0; i < jumlahPool; i++)
        {
            GameObject obj = Instantiate(pool);
            obj.SetActive(false);
            obj.transform.SetParent(platformsParent);
            objectPools.Add(obj);
        }

    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < objectPools.Count; i++)
        {
            if (!objectPools[i].activeInHierarchy)
            {
                return objectPools[i];
            }
        }

        GameObject obj = Instantiate(pool);
        obj.transform.SetParent(platformsParent);
        obj.SetActive(false);
        objectPools.Add(obj);

        return obj;
    }
    

}
