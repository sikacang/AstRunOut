using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] float lifeTime = 3.5f;
    public float damage = 0.5f;
    public GameObject particle;

    private void Update()
    {
        if (lifeTime < 0)
            Destroy(gameObject);

        lifeTime -= Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerData.Instance.TakeDamage(damage);
            AudioManager.Instance.Play("PlayerHit");
            GameObject particleEffect = Instantiate(particle, transform.position, transform.rotation);
            Destroy(particleEffect, 0.2f);
            Destroy(gameObject);
        }
    }
}
