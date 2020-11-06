using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : Gun
{
    Animator _animator;

    public GameObject bulletPrefab;
    public Transform gunTip;
    public float bulletForce;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public override void Shoot()
    {
        base.Shoot();
        _animator.SetTrigger("onShoot");

        Fire();
    }

    void Fire()
    {
        AudioManager.Instance.Play("PlayerShoot");
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, gunTip.position, gunTip.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(gunTip.up * bulletForce, ForceMode2D.Impulse);
    }
}
