using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Gun : MonoBehaviour
{
    protected RaycastHit2D hit;
    Vector2 direction;
    float angle;

    [Header("Gun Data")]
    public float fireRate;
    float nextFire;

    private void Update()
    {
        if (Input.GetMouseButton(0) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Shoot();
        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = (mousePosition - (Vector2)transform.position).normalized;

        angle = Mathf.Atan2(direction.y, direction.x);
        if (angle < 0f)
        {
            angle = Mathf.PI * 2 + angle;
        }

        Vector3 aimDirection = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg) * Vector2.right;

        transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg - 90);
    }

    public virtual void Shoot() { }

}
