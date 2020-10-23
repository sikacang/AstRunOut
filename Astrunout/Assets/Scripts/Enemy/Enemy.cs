using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float health;
    public float speed;
    public State _enemyState;

    [Header("Attack Properties")]
    public GameObject bulletPrefab;
    public float fireRate;
    public float bulletForce;
    public float enemyShootPlayerRange;
    float nextFire;

    private GameObject player;
    private Vector2 screenHalfWidht;
    private Vector2 roamingPosition;
    public enum State
    {
        Chasing,
        Attack
    }

    private void Start()
    {
        _enemyState = State.Chasing;
        player = GameObject.FindGameObjectWithTag("Player");
        screenHalfWidht = new Vector2(
                Camera.main.aspect * Camera.main.orthographicSize, 
                Camera.main.orthographicSize
            );
        roamingPosition = new Vector2(
                Random.Range(-screenHalfWidht.x, screenHalfWidht.x), (screenHalfWidht.y - 2)
            );
    }

    private void Update()
    {
        if (player == null) return;

        switch (_enemyState)
        {
            case State.Chasing:
                Vector2 playerPosition = player.transform.position;
                transform.position = Vector2.MoveTowards(transform.position, playerPosition,  speed * Time.deltaTime);
                if(transform.position.y < (screenHalfWidht.y - 2))
                {
                    _enemyState = State.Attack;
                }
                break;

            case State.Attack:
                Patrol();
                Attack();
                break;
        }

        CheckDead();
    }

    void Patrol()
    {
        transform.localPosition = Vector2.MoveTowards(transform.localPosition, roamingPosition, speed * Time.deltaTime);

        if(Vector2.Distance(transform.localPosition, roamingPosition) < 1f)
        {
            roamingPosition = new Vector2(
                Random.Range(-screenHalfWidht.x, screenHalfWidht.x), transform.position.y
            );
        }
    }

    void Attack()
    {
        if(Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Vector3 playerPosition = player.transform.position;
            Vector3 playerFace = player.transform.right;
            Vector3 shootDirection = playerPosition + (playerFace * enemyShootPlayerRange);

            Vector3 bulletDirection = (shootDirection - transform.position).normalized;

            AudioManager.Instance.Play("EnemyShoot");

            GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(bulletDirection * bulletForce, ForceMode2D.Impulse);
        }
    }

    void CheckDead()
    {
        if(health <= 0)
        {
            AudioManager.Instance.Play("EnemyDestroyed");
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float hit)
    {
        health -= hit;
    }
}
