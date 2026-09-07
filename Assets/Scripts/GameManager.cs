using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { Playing, GameOver, Won }
    private GameState currentState;

    [Header("References")]
    [SerializeField] private CollectibleManager collectibleManager;

    [Header("UI - Gameplay")]
    [SerializeField] private Text scoreText;

    [Header("UI - Game Over")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("UI - Win")]
    [SerializeField] private GameObject winPanel;

    private int score;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        currentState = GameState.Playing;
        score = 0;

        UpdateScoreUI();

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Game started.");
    }

    public void AddScore(int amount)
    {
        if (currentState != GameState.Playing) return;

        score += amount;
        UpdateScoreUI();

        Debug.Log("Score updated: " + score);
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Scrap: " + score;
        }
    }

    public void GameOver()
    {
        if (currentState != GameState.Playing) return;

        currentState = GameState.GameOver;
        Debug.Log("Game Over.");

        if (gameOverPanel != null) gameOverPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    public void CheckWinCondition()
    {
        if (currentState != GameState.Playing) return;
        if (collectibleManager != null && collectibleManager.RemainingItems > 0) return;

        currentState = GameState.Won;
        Debug.Log("Player won: all collectibles gathered.");

        if (winPanel != null) winPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f; 
        Debug.Log("Restarting level.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
    }
}