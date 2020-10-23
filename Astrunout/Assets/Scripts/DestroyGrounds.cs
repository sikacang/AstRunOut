using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGrounds : MonoBehaviour
{
    private GameObject destructionPoint;

    private void Awake()
    {
        destructionPoint = GameObject.Find("destructionPoint");
    }

    private void Update()
    {
        if(transform.position.x < destructionPoint.transform.position.x)
        {
            gameObject.SetActive(false);
        }
    }
}
