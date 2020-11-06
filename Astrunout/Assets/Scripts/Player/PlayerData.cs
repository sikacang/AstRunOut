using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    #region "Singleton"
    public static PlayerData Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public float health;
    float healthCount;

    private void Start()
    {
        healthCount = health;
        UIManager.Instance.healthBar.SetMaxAmount(health);
    }

    public void TakeDamage(float amount)
    {
        healthCount -= amount;
        UIManager.Instance.healthBar.SetAmount(healthCount);

        CheckDead();
    }

    public void CheckDead()
    {
        if(healthCount <= 0)
        {
            GameManager.Instance.GameOver();
            Destroy(gameObject);
        }
    }
}
