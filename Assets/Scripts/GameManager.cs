using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI scoreText = null;

    public static GameManager instance;
    public delegate void StartController();
    public static event StartController OnInitialized;
    public static event StartController OnGameOver;
    public static event StartController OnReset;

    private int score = 0;
    private bool isInitialized = false;

    [Header("GameOver Inputs")]
    [SerializeField]
    private GameObject gameOverPanel = null;
    [SerializeField]
    private TMPro.TextMeshProUGUI messageText = null;
    [SerializeField]
    [TextArea]
    private string winText = string.Empty;
    [SerializeField]
    [TextArea]
    private string loseText = string.Empty;

    void Awake()
    {
        if (instance == null)//we make GameManager singleton for easy access
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public void OnStartGame()
    {
        //here we trigger the event to initialize the game and avoid everything starts on Play
        if (OnInitialized != null && !isInitialized)
        {
            OnInitialized();
            isInitialized = true;
        }
    }

    public void GameOver(bool playerWon)
    {
        //Event to trigger the Game Over process
        if (OnGameOver != null)
        {
            OnGameOver();
        }

        isInitialized = false;
        if (playerWon)
        {
            messageText.text = winText;
        }

        else
        {
            messageText.text = loseText;
        }

        gameOverPanel.SetActive(true);
    }

    public void ResetGame()
    {
        //Event to Reset the game, delete enemies, reset score and reset flags
        if (OnReset != null)
        {
            OnReset();
        }

        gameOverPanel.gameObject.SetActive(false);

        isInitialized = false;

        score = 0;
        scoreText.text = "Score: " + score;

        OnStartGame();
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }
}
