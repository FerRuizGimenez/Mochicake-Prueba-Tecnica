using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CameraColor cameraColor;
    public bool gameStarted;
    public GameObject platformSpawner;
    public GameObject gameplayUI;
    public GameObject menuUi;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI diamondsText;
    public TextMeshProUGUI totalDiamondsText;
    AudioSource audioSource;
    public AudioClip[] gameAudio;

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
        //PlayerPrefs.SetInt("Diamonds", 0);

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

        //play audio
        audioSource.PlayOneShot(gameAudio[0]);
        cameraColor.StartColorChange();

        StartCoroutine("UpdateScore");
    }
    public void GameOver()
    {
        gameStarted = false;

        platformSpawner.SetActive(false);
        StopCoroutine("UpdateScore");
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
            //print(score);
        }
    }
    public void CollectDiamonds()
    {
        diamonds += 1;
        diamondsText.text = diamonds.ToString();
        
        audioSource.PlayOneShot(gameAudio[2], 0.3f);
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

    void SaveDiamonds()
    {
        int savedDiamonds = PlayerPrefs.GetInt("Diamonds", 0);
        PlayerPrefs.SetInt("Diamonds", savedDiamonds + diamonds);
    }
}