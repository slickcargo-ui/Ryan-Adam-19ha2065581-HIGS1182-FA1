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
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;

    [Header("UI - High Score")]
    [SerializeField] private TMPro.TextMeshProUGUI highScoreText;
    [SerializeField] private TMPro.TextMeshProUGUI LosehighScoreText;

    [Header("UI - Game Over")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("UI - Win")]
    [SerializeField] private GameObject winPanel;

    [Header("UI - Difficulty")]
    [SerializeField] private TMPro.TextMeshProUGUI difficultyText;

    [Header("spawners")]
    [SerializeField] private AsteroidsSpawner asteroidsSpawner;
    [SerializeField] private EnemySpawner enemySpawner;

    [Header("Effects")]
    [SerializeField] private GameObject destroyEffectPrefab;
    [SerializeField] private GameObject ScrapPickupEffect;

    private int score;
    public int DifficultyLevel = 1;
    private float timeUntilIncrease = 30f; // Time in seconds until difficulty increases

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

    public void Update()
    {
        if (currentState != GameState.Playing) return;

        timeUntilIncrease -= Time.deltaTime;
        if (timeUntilIncrease <= 0f)
        {
            IncreaseDifficulty();
            timeUntilIncrease = 30f; // Reset the timer
        }
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

    public void IncreaseDifficulty()
    {
               DifficultyLevel++;

        if (asteroidsSpawner != null)
        {
            asteroidsSpawner.AsteroidDiffIncrease();
        }

        if (enemySpawner != null)
        {
            enemySpawner.EnemyDiffIncrease();
        }

        if (difficultyText != null)
        {
            difficultyText.text = "Difficulty: " + DifficultyLevel;
        }

        Debug.Log("Difficulty increased to: " + DifficultyLevel);
    }

    public void AddScore(int amount)
    {
        if (currentState != GameState.Playing) return;

        score += amount;
        UpdateScoreUI();

        Debug.Log("Score updated: " + score);
    }

    public void DestroyEffect(Vector3 position)
    {
      Instantiate(destroyEffectPrefab, position, Quaternion.identity);
        Debug.Log("Destroy effect instantiated at: " + position);
    }

    public void PickupEffect(Vector3 position)
    {
       Instantiate(ScrapPickupEffect, position, Quaternion.identity);
        Debug.Log("Pickup effect instantiated at: " + position);
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
        SaveManager.SaveHighScore(score);

        if (LosehighScoreText != null)
        {
            LosehighScoreText.text = "High Score: " + SaveManager.LoadHighScore();
        }

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
        SaveManager.SaveHighScore(score);

        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + SaveManager.LoadHighScore();
        }

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