using UnityEngine;
 static class SaveManager
{
    private const string HighScoreKey = "HighScore";
    public static void SaveHighScore(int score)
    {
        if (score > LoadHighScore())
        {
            PlayerPrefs.SetInt(HighScoreKey, score);
            PlayerPrefs.Save();
            Debug.Log("High score saved: " + score);
        }
    }
   public static int LoadHighScore()
    {
        int highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        Debug.Log("High score loaded: " + highScore);
        return highScore;
    }
}
