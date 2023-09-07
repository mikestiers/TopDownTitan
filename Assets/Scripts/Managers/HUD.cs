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
    public GameObject waveCanvas;
    public Text waveText;
    public Transform weaponSelectorGrid;
    public Text shieldText;
    public GameObject weaponButtonPrefab;
    public Button pauseButton;

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
    private List<WeaponButton> weaponButtons = new List<WeaponButton>();

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
        Time.timeScale = 1f;
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

    public void CreateWeaponButton(Weapon weapon, int index)
    {
        WeaponButton button = Instantiate(weaponButtonPrefab, weaponSelectorGrid).GetComponent<WeaponButton>();
        button.Initialize(weapon, index);
        weaponButtons.Add(button);
    }

    public void RemoveWeaponButton(Weapon weapon, int index)
    {
        WeaponButton button = weaponButtons[index];
        weaponButtons.Remove(button); // only weapon of the script gets removed
        button.Remove();
        // weaponButtons.RemoveAt(index);
        // Destroy(button); // Destroys the script attached
    }

    public void OnSelectWeaponIndex(int index)
    {
        for (int i = 0; i < weaponButtons.Count; i++)
        {
            weaponButtons[i].iconImage.color = Color.white;
        }
        weaponButtons[index].iconImage.color = weaponButtons[index].weapon.color;
    }
}
