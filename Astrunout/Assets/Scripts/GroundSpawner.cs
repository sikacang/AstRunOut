using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public Transform batasSpawn;

    public Vector2 offset;

    private float[] groundWidth;

    public ObjectPooler[] _objectPooler;

    public Transform maxHeightPoint;
    private float heightChange;
    public float maxHeightChange;
    private float maxHeight;
    private float minHeight;

    private void Awake()
    {
        groundWidth = new float[_objectPooler.Length];

        for(int i = 0; i<_objectPooler.Length; i++)
        {
            groundWidth[i] = _objectPooler[i].pool.GetComponent<SpriteRenderer>().bounds.size.x;
        }

        minHeight = transform.position.y;
        maxHeight = maxHeightPoint.position.y;
    }

    private void Update()
    {
        if(transform.position.x < batasSpawn.position.x)
        {
            float jarakPlatform = Random.Range(offset.x, offset.y);
            int selectGround = Random.Range(0, _objectPooler.Length);

            heightChange = transform.position.y + Random.Range(maxHeightChange, -maxHeightChange);

            if (heightChange > maxHeight) heightChange = maxHeight;
            else if (heightChange < minHeight) heightChange = minHeight;

            transform.position = new Vector3(transform.position.x + (groundWidth[selectGround]/2) + jarakPlatform, heightChange);

            GameObject platforms = _objectPooler[selectGround].GetPooledObject();
            platforms.transform.position = transform.position;
            platforms.transform.rotation = transform.rotation;
            platforms.SetActive(true);

            transform.position = new Vector3(transform.position.x + (groundWidth[selectGround] / 2), transform.position.y);
        }
    }
}
