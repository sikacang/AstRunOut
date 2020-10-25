using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region SINGLETON
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public float gameSpeed;

    [Header("Difficulty Settings")]
    public float speedIncreaseMilestone;
    public float milestone;
    public float speedMultiplier;

    bool gameEnd = false;
    bool isPaused = false;
    int i = 1;
    private void Update()
    {
        if(transform.position.x > milestone)
        {
            milestone += speedIncreaseMilestone;
            speedIncreaseMilestone += speedIncreaseMilestone * speedMultiplier;
            gameSpeed *= speedMultiplier;
            print(i++);
            if(EnemySpawner.Instance.spawnTime >= 1.6f)
                EnemySpawner.Instance.spawnTime /= speedMultiplier;
        }

        transform.position += Vector3.right * gameSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                Pause();
                isPaused = true;
            }
            else if (isPaused)
            {
                Resume();
                isPaused = false;
            }
        }
    }

    public void RestartGame()
    {
        AudioManager.Instance.Play("ClickSound");
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Pause()
    {
        UIManager.Instance.PauseGame();
        Time.timeScale = 0;
    }

    public void Resume()
    {
        UIManager.Instance.Resume();
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        AudioManager.Instance.Play("ClickSound");
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        AudioManager.Instance.Play("ClickSound");
        SceneManager.LoadScene(1);
    }

    public void GameOver()
    {
        gameEnd = true;
        UIManager.Instance.GameOverScreen();
        ScoreManager.Instance.isScoring = false;
        Time.timeScale = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            UIManager.Instance.healthBar.SetAmount(0);
            Invoke("GameOver", .5f);
        }
    }

    public bool IsEnded()
    {
        return gameEnd;
    }
}
