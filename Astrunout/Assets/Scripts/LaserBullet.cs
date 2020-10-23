using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBullet : MonoBehaviour
{
    public float lifeTime;
    public GameObject particle;

    private void Update()
    {
        if (lifeTime < 0)
            Destroy(gameObject);

        lifeTime -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(1);
            GameObject particleEffect = Instantiate(particle, transform.position, transform.rotation);
            Destroy(particleEffect, 0.2f);
            Destroy(gameObject);
        }
    }
}
