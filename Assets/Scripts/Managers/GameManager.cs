using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private PlayerController player;
    public AudioClip music;
    public Transform playerTransform;
    public Transform spawnPosition;
    public Text scoreText;
    public int score = 0;
    private int highScore = 0;
    public int lives;
    public int shields;

    private void Start()
    {
        AudioManager.singleton.PlayMusic(music);
        //GameManager.singleton.score = 0;
        //HUD.singleton.UpdateScore(0);
        //GameManager.singleton.shields = 1;
        //HUD.singleton.UpdateShields(GameManager.singleton.shields);
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        HUD.singleton.UpdateScore(score);
    }

    public void GameOver()
    {
        SceneManager.LoadScene("Menu");
    }

    public bool IsHighScore(int score)
    {
        if (score > highScore)
        {
            highScore = score;
            return true;
        }
        else
            return false;
    }
}
