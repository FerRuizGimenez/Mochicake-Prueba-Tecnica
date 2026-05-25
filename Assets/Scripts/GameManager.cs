using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool gameStarted;
    public GameObject platformSpawner;

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
}