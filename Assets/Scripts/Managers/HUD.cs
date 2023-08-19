using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : Singleton<HUD>
{
    [Header("Game HUD")]
    public GameObject gameHudCanvas;
    public Text livesText;
    public Text scoreText;
    public Text highScoreText;
    public GridLayoutGroup weaponSelectorGrid;
    public Text shieldText;
    public GameObject weaponButtonPrefab;
    private Dictionary<string, WeaponButton> weaponButtons = new Dictionary<string, WeaponButton>();

    [Header("Pause Menu")]
    public GameObject pauseMenuCanvas;
    public Button resumeButton;
    public Button quitButton;
    public Button musicOnOffButton;
    public Slider musicVolumeSlider;
    public Button soundOnOffbutton;
    public Slider soundVolumeSlider;

    [Header("Game Over Menu")]
    public GameObject gameOverMenuCanvas;
    public Button restartButton;

    void Start()
    {
        resumeButton.onClick.AddListener(Resume);
        quitButton.onClick.AddListener(Quit);
        musicOnOffButton.onClick.AddListener(MuteMusic);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        soundOnOffbutton.onClick.AddListener(MuteSound);
        soundVolumeSlider.onValueChanged.AddListener(SetSoundVolume);
        restartButton.onClick.AddListener(Quit);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Pause()
    {
        gameHudCanvas.SetActive(false);
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    void Resume()
    {
        pauseMenuCanvas.SetActive(false);
        gameHudCanvas.SetActive(true);
        Time.timeScale = 1f;
    }

    void Quit()
    {
        HUD.singleton.gameOverMenuCanvas.SetActive(false);
        SceneManager.UnloadSceneAsync("Game");
        SceneManager.LoadScene("Menu");
    }

    void MuteMusic()
    {
        AudioManager.singleton.SetMusicVolume(musicVolumeSlider.value);
    }

    void MuteSound()
    {
        AudioManager.singleton.SetSoundEffectVolume(soundVolumeSlider.value);
    }

    void SetMusicVolume(float volume)
    {
        AudioManager.singleton.SetMusicVolume(volume);
    }

    void SetSoundVolume(float volume)
    {
        AudioManager.singleton.SetSoundEffectVolume(volume);
    }

    public void UpdateShields(int shields)
    {
        shieldText.text = $"Shields\n{shields}";
    }

    public void UpdateScore(int score)
    {
        scoreText.text = $"Score\n{score.ToString()}";
        if (GameManager.singleton.IsHighScore(score))
            UpdateHighScore(score);
    }

    public void UpdateHighScore(int score)
    {
        highScoreText.text = $"High Score\n{score.ToString()}";
    }

    public void AddWeapon(Weapon newWeaponPrefab, Sprite newWeaponIcon)
    {
        if (!weaponButtons.ContainsKey(newWeaponPrefab.name))
        {
            GameObject newButton = Instantiate(weaponButtonPrefab, weaponSelectorGrid.transform);
            WeaponButton weaponButton = newButton.GetComponent<WeaponButton>();
            weaponButton.SetWeapon(newWeaponPrefab, newWeaponIcon);

            weaponButtons.Add(newWeaponPrefab.name, weaponButton);
        }
    }
}
