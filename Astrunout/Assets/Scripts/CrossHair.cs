using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    Vector3 position;
    bool isEnd;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        isEnd = GameManager.Instance.IsEnded();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnd)
        {
            position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(position.x, position.y, 0);
            isEnd = GameManager.Instance.IsEnded();
        }
        else if (isEnd)
        {
            Cursor.visible = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
