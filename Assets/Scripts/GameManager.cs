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
    public TextMeshProUGUI scoreText;

    int score = 0;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
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

        StartCoroutine(UpdateScore());
    }
    public void GameOver()
    {
        platformSpawner.SetActive(false);

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
}