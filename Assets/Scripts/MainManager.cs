using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;  // Prefab for the brick objects in the game
    public int LineCount = 6;  // Number of lines of bricks
    public Rigidbody Ball;  // Rigidbody component of the ball

    public Text ScoreText;  // Text component for displaying current score
    public Text ScoreText1;  // Text component for displaying high score
    public GameObject GameOverText;  // Text/Panel displayed when the game is over

    private bool m_Started = false;  // Flag to check if the game has started
    private int m_Points;  // Current score points

    private bool m_GameOver = false;  // Flag to check if the game is over

    void Start()
    {
        LoadHighScore();  // Load the high score from MainGameManager
        ScoreText1.text = $"Best Score : {MainGameManager.Instance.playerName} : {MainGameManager.Instance.bestScore}";

        // Set up the game environment
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        // Instantiate bricks based on the LineCount and perLine calculation
        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    void Update()
    {
        // Start the game on space key press if it hasn't started yet
        if (!m_Started && Input.GetKeyDown(KeyCode.Space))
        {
            m_Started = true;
            float randomDirection = Random.Range(-1.0f, 1.0f);
            Vector3 forceDir = new Vector3(randomDirection, 1, 0);
            forceDir.Normalize();

            Ball.transform.SetParent(null);
            Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
        }
        else if (m_GameOver && Input.GetKeyDown(KeyCode.Space))
        {
            // Restart the game when it is over on space key press
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void AddPoint(int point)
    {
        // Update score and check for high score updates
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
        if (m_Points > MainGameManager.Instance.bestScore)
        {
            MainGameManager.Instance.bestScore = m_Points;
            MainGameManager.Instance.playerName = PlayerPrefs.GetString("CurrentPlayerName", "Player"); // Update high score holder's name
            MainGameManager.Instance.SaveNameAndScore();
            UpdateHighScoreDisplay();
        }
    }

    public void GameOver()
    {
        // Mark the game as over and update UI
        m_GameOver = true;
        GameOverText.SetActive(true);
        UpdateHighScoreDisplay();
    }

    void LoadHighScore()
    {
        // Load high score from MainGameManager
        MainGameManager.Instance.LoadNameAndScore();
        UpdateHighScoreDisplay();
    }

    void UpdateHighScoreDisplay()
    {
        // Update UI for score and high score
        ScoreText.text = $"Score : {m_Points}";
    }
}
