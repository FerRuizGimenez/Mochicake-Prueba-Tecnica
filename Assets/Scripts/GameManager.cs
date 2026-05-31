using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Singleton instance accessible from any script
    public static GameManager instance;
    public bool gameStarted;

    [Header("Game Objects")]
    public GameObject platformSpawner;
    public GameObject gameplayUI;
    public GameObject menuUi;
    public GameObject gameOverUI;
    public GameObject floatingTextPrefab;

    [Header("UI Text")]
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI totalDiamondsText;
    public TextMeshProUGUI gameOverScoreText;
    public TextMeshProUGUI gameOverDiamondsText;

    [Header("References")]
    public ColorManager colorManager;
    public PlayerController player;

    [Header("Audio")]
    AudioSource audioSource;
    public AudioClip[] gameAudio;

    [Header("Difficulty")]
    public float speedIncreaseAmount = 0.5f;
    public float speedIncreaseInterval = 10f;

    private int score = 0;
    private int highScore;
    private int diamonds;
    private int totalDiamonds;

    void Awake()
    {
        // Ensure only one instance of GameManager exists
        if(instance == null)
        {
            instance = this;
        }
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        // Load and display saved high score and total diamonds
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
                // If game over screen is active, restart; otherwise start the game
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

    // Play a sound by index from the gameAudio array with optional volume
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

        // Show game over screen with final stats
        gameplayUI.SetActive(false);
        gameOverUI.SetActive(true);
        gameOverScoreText.text = "Final Score: " + score;
        gameOverDiamondsText.text = "Collected Diamonds: " + diamonds;
    }

    void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    // Increment score every second while the game is running
    IEnumerator UpdateScore()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            score++;
            scoreText.text = score.ToString();
        }
    }

    // Gradually increase player speed over time to raise difficulty
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
        PlaySound(2, 0.1f);

        // Spawn floating +1 text slightly above the diamond position
        Vector3 spawnPos = position;
        spawnPos.y += 1.5f;
        Instantiate(floatingTextPrefab, spawnPos, Quaternion.identity);
    }

    // Save high score only if current score exceeds the previous best
    void SaveHighScore()
    {
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }

    // Accumulate diamonds across sessions
    void SaveDiamonds()
    {
        int savedDiamonds = PlayerPrefs.GetInt("Diamonds", 0);
        PlayerPrefs.SetInt("Diamonds", savedDiamonds + diamonds);
    }
}