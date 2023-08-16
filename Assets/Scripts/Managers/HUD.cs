using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("Game HUD")]
    public GameObject gameHudCanvas;
    public Text livesText;
    public Text scoreText;
    public Text highScoreText;
    public Grid weaponSelectorGrid;
    public Text healthText;
    
    [Header("Pause Menu")]
    public GameObject pauseMenuCanvas;
    public Button resumeButton;
    public Button quitButton;
    public Button musicOnOffButton;
    public Slider musicVolumeSlider;
    public Button soundOnOffbutton;
    public Slider soundVolumeSlider;

    void Start()
    {
        resumeButton.onClick.AddListener(Resume);
        quitButton.onClick.AddListener(Quit);
        musicOnOffButton.onClick.AddListener(SetMusic);
        soundOnOffbutton.onClick.AddListener(SetSound);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            gameHudCanvas.SetActive(false);
            pauseMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    void Resume()
    {
        pauseMenuCanvas.SetActive(false);
        gameHudCanvas.SetActive(true);
        Time.timeScale = 1f;
    }

    void Quit()
    {
        SceneManager.LoadScene("Menu");
    }

    void SetMusic()
    {

    }

    void SetSound()
    {

    }
}
