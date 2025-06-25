using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject startButton;
    public GameObject gameOverText;

    private bool gameStarted = false;
    private bool isGameOver = false;

    public float gameOverHeight = 6f;

    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        Time.timeScale = 0f;
        gameOverText.SetActive(false);
    }
    
    void Update()
    {
        if (isGameOver) return;
    }
    
    public bool IsGameStarted()
    {
        return gameStarted && !isGameOver;
    }

    public void StartGame()
    {
        gameStarted = true;
        Time.timeScale = 1f;
        startButton.SetActive(false);
        
        FruitSpawner.Instance.SpawnNewFruit();
    }
    
    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        gameOverText.SetActive(true);
    }
}