using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Text scoreText;
    public Text highScoreText;

    private float score;
    private float highScore;

    public float pointPerSecond;
    public bool isScoring;

    private void Start()
    {
        highScore = PlayerPrefs.GetFloat("Highscore");
    }

    private void Update()
    {
        //tambah score per waktu
        if (isScoring)
        { 
            score += pointPerSecond * Time.deltaTime;
        }
        
        if(score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetFloat("Highscore", highScore);
        }

        //update UI score
        scoreText.text = Mathf.Round(score).ToString();
        highScoreText.text = Mathf.Round(PlayerPrefs.GetFloat("Highscore")).ToString();
    }

    public void AddScore(float point)
    {
        score += point;
    }

    public float GetCurrentScore()
    {
        return score;
    }
}
