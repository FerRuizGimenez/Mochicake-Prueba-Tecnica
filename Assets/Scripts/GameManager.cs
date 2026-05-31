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
    public GameObject gameOverUI;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI totalDiamondsText;
    public TextMeshProUGUI gameOverScoreText;
    public TextMeshProUGUI gameOverDiamondsText;
    public GameObject floatingTextPrefab;
    public ColorManager colorManager;
    public PlayerController player;

    AudioSource audioSource;
    public AudioClip[] gameAudio;

    public float speedIncreaseAmount = 0.5f;
    public float speedIncreaseInterval = 10f;

    int score = 0;
    int highScore;
    int diamonds;
    int totalDiamonds;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "Best Score : " + highScore;
        totalDiamonds = PlayerPrefs.GetInt("Diamonds", 0);
        totalDiamondsText.text = totalDiamonds.ToString();
    }

    void Update()
    {
        if (!gameStarted)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (gameOverUI.activeSelf)
                {
                    Restart();
                }
                else
                {
                    GameStart();
                }
            }
        }
    }

    public void GameStart()
    {
        gameStarted = true;
        platformSpawner.SetActive(true);
        gameplayUI.SetActive(true);
        menuUi.SetActive(false);
        PlaySound(0, 0.2f);
        colorManager.StartColorChange();
        StartCoroutine("UpdateScore");
        StartCoroutine("IncreaseSpeed");
    }

    public void PlaySound(int index, float volume = 1f)
    {
        audioSource.PlayOneShot(gameAudio[index], volume);
    }

    public void GameOver()
    {
        gameStarted = false;
        platformSpawner.SetActive(false);
        StopCoroutine("UpdateScore");
        StopCoroutine("IncreaseSpeed");
        colorManager.StopColorChange();
        PlaySound(3, 0.03f);
        SaveHighScore();
        SaveDiamonds();

        gameplayUI.SetActive(false);
        gameOverUI.SetActive(true);
        gameOverScoreText.text = "Final Score: " + score;
        gameOverDiamondsText.text = "Collected Diamonds: " + diamonds;
    }

    void Restart()
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
        }
    }

    IEnumerator IncreaseSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(speedIncreaseInterval);
            player.IncreaseSpeed(speedIncreaseAmount);
        }
    }

    public void CollectDiamonds(Vector3 position)
    {
        diamonds += 1;
        PlaySound(2, 0.3f);

        Vector3 spawnPos = position;
        spawnPos.y += 1.5f;
        Instantiate(floatingTextPrefab, spawnPos, Quaternion.identity);
    }

    void SaveHighScore()
    {
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }

    void SaveDiamonds()
    {
        int savedDiamonds = PlayerPrefs.GetInt("Diamonds", 0);
        PlayerPrefs.SetInt("Diamonds", savedDiamonds + diamonds);
    }
}