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
    public TextMeshProUGUI totalDiamondsText;
    public GameObject floatingTextPrefab;
    public CameraColor cameraColor;
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
        audioSource.PlayOneShot(gameAudio[0]);
        cameraColor.StartColorChange();
        StartCoroutine("UpdateScore");
        StartCoroutine("IncreaseSpeed");
    }

    public void GameOver()
    {
        gameStarted = false;
        platformSpawner.SetActive(false);
        StopCoroutine("UpdateScore");
        StopCoroutine("IncreaseSpeed");
        SaveHighScore();
        SaveDiamonds();
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
        audioSource.PlayOneShot(gameAudio[2], 0.3f);

        Vector3 spawnPos = position;
        spawnPos.y += 3f;
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