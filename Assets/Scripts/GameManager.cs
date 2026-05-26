using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool gameStarted;
    public GameObject platformSpawner;
    public GameObject gameplayUI;
    public GameObject menuUi;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI scoreText;

    int score = 0;
    int highScore;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore");
        highScoreText.text = "Best Score : " + highScore;
    }
    void Update()
    {
        if (!gameStarted)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameStart();
            }
        }
    }
    public void GameStart()
    {
        gameStarted = true;
        platformSpawner.SetActive(true);
        gameplayUI.SetActive(true);
        menuUi.SetActive(false);

        StartCoroutine("UpdateScore");
    }
    public void GameOver()
    {
        platformSpawner.SetActive(false);
        StopCoroutine("UpdateScore");
        SaveHighScore();
        Invoke("ReloadLevel", 1f);
    }
    void ReloadLevel()
    {
        SceneManager.LoadScene("Game");
    }
    IEnumerator UpdateScore()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            score++;
            scoreText.text = score.ToString(); 
            //print(score);
        }
    }
    void SaveHighScore()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            //already have a highscore - not playing for the first time
            if(score > PlayerPrefs.GetInt("HighScore"))
            {
                PlayerPrefs.SetInt("HighScore", score);
            }
        }
        else
        {
            //playing for the first time
            PlayerPrefs.SetInt("HighScore", score); 
        }
    }
}