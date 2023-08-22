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
    public Button pauseButton;
    private Dictionary<string, WeaponButton> weaponButtons = new Dictionary<string, WeaponButton>();
    private WeaponButton activeWeapon;

    [Header("Pause Menu")]
    public GameObject pauseMenuCanvas;
    public Button resumeButton;
    public Button quitButton;
    public Button musicOnOffButton;
    public Slider musicVolumeSlider;
    public Button soundOnOffButton;
    public Slider soundVolumeSlider;

    [Header("Game Over Menu")]
    public GameObject gameOverMenuCanvas;
    public Button restartButton;

    private float lastMusicVolume = 0.25f;
    private float lastSoundVolume = 0.25f;

    void Start()
    {
        pauseButton.onClick.AddListener(Pause);
        resumeButton.onClick.AddListener(Resume);
        quitButton.onClick.AddListener(Quit);
        musicOnOffButton.onClick.AddListener(MuteMusic);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        soundOnOffButton.onClick.AddListener(MuteSound);
        soundVolumeSlider.onValueChanged.AddListener(SetSoundVolume);
        restartButton.onClick.AddListener(Quit);
        UpdateShields(GameManager.singleton.shields);
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
        Destroy(gameObject);
        Destroy(GameManager.singleton.gameObject);
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in allAudioSources)
        {
            audio.Stop();
        }
        SceneManager.UnloadSceneAsync("Game");
        SceneManager.LoadScene("Menu");
    }

    void MuteMusic()
    {
        // Find the child object's Text component
        Text childText = musicOnOffButton.transform.Find("Text").GetComponent<Text>();

        if (musicVolumeSlider.value > 0) // If the current volume is greater than 0, mute it
        {
            lastMusicVolume = musicVolumeSlider.value; // Store the current volume before muting
            musicVolumeSlider.value = 0;
            AudioManager.singleton.SetMusicVolume(0);
            childText.text = "Unmute Music"; // Update the text to indicate the music is muted
        }
        else // If the current volume is 0, restore to the last known volume
        {
            musicVolumeSlider.value = lastMusicVolume;
            AudioManager.singleton.SetMusicVolume(lastMusicVolume);
            childText.text = "Mute Music"; // Update the text to indicate the music is playing
        }
    }

    void MuteSound()
    {
        Text childText = soundOnOffButton.transform.Find("Text").GetComponent<Text>();

        if (soundVolumeSlider.value > 0) // If the current volume is greater than 0, mute it
        {
            lastSoundVolume = soundVolumeSlider.value; // Store the current volume before muting
            soundVolumeSlider.value = 0;
            AudioManager.singleton.SetSoundEffectVolume(0);
            childText.text = "Unmute Sound"; // Update the text to indicate the music is muted
        }
        else // If the current volume is 0, restore to the last known volume
        {
            soundVolumeSlider.value = lastSoundVolume;
            AudioManager.singleton.SetSoundEffectVolume(lastSoundVolume);
            childText.text = "Mute Sound"; // Update the text to indicate the music is playing
        }
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
        else
        {
            // If the weapon already exists, set it as the active one
            SetActiveIcon(weaponButtons[newWeaponPrefab.name]);
        }

    }

    public void SetActiveIcon(WeaponButton newActiveIcon)
    {
        // Reset active icon to unselected colors
        if (activeWeapon != null)
        {
            activeWeapon.iconImage.color = Color.white; // Use iconImage instead of buttonPrefab
        }

        // Set the new active icon and update the color
        activeWeapon = newActiveIcon;
        activeWeapon.iconImage.color = Color.green;

    }
}
