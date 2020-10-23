using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region SINGLETON
    public static UIManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public BarItem rocketBar;
    public BarItem healthBar;

    [Header("Game Over UI")]
    public GameObject gameOverScreen;
    public Text scoreUI;

    [Header("Pause screen UI")]
    public GameObject pauseScreen;

    public void GameOverScreen()
    {
        gameOverScreen.SetActive(true);
        scoreUI.text = Mathf.Round(ScoreManager.Instance.score).ToString();
    }

    public void PauseGame()
    {
        pauseScreen.SetActive(true);
    }

    public void Resume()
    {
        pauseScreen.SetActive(false);
    }
}

[System.Serializable]
public class BarItem
{
    public Slider slider;

    public float maxRocket;
    public float currentAmount;

    public void SetMaxAmount(float amount)
    {
        slider.maxValue = amount;
        slider.value = amount;
    }

    public void SetAmount(float amount)
    {
        slider.value = amount;
    }
}
