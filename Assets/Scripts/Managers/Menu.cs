using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Header("Main Menu")]
    public GameObject mainCanvas;
    public Button playButton;
    public Button instructionsButton;
    public Button creditsButton;
    public Button highScoresButton;
    public GameObject mainMenuCharacter;
    private float hoverRange = 0.5f;
    private float hoverSpeed = 1.0f;
    private Vector3 initialPosition;

    [Header("Instructions Menu")]
    public GameObject instructionsCanvas;
    public Button instructionsBackButton;

    [Header("Credits Menu")]
    public GameObject creditsCanvas;
    public Button creditsBackButton;

    [Header("High Scores Menu")]
    public GameObject highScoresCanvas;
    public Button highScoresBackButton;

    [Header("Audio")]
    public AudioClip music;

    void Start()
    {
        playButton.onClick.AddListener(Play);
        instructionsButton.onClick.AddListener(InstructionsMenu);
        creditsButton.onClick.AddListener(CreditsMenu);
        highScoresButton.onClick.AddListener(HighScoresMenu);
        instructionsBackButton.onClick.AddListener(BackToMain);
        creditsBackButton.onClick.AddListener(BackToMain);
        highScoresBackButton.onClick.AddListener(BackToMain);
        AudioManager.singleton.PlayMusic(music);
        AudioManager.singleton.SetMusicVolume(25.0f);
        AudioManager.singleton.SetSoundEffectVolume(25.0f);
        initialPosition = mainMenuCharacter.transform.position;
    }

    private void Update()
    {
        // Move the character on the main screen around a bit
        float newYPosition = initialPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverRange;
        mainMenuCharacter.transform.position = new Vector3(initialPosition.x, newYPosition, initialPosition.z);
    }

    void Play()
    {
        // ScreenFlash.cs is called
        //SceneManager.LoadScene("Game");
        //Time.timeScale = 1f;
    }

    void InstructionsMenu()
    {
        mainCanvas.SetActive(false);
        instructionsCanvas.SetActive(true);
    }

    void CreditsMenu()
    {
        mainCanvas.SetActive(false);
        creditsCanvas.SetActive(true);
    }

    void HighScoresMenu()
    {
        mainCanvas.SetActive(false);
        highScoresCanvas.SetActive(true);
    }

    void BackToMain()
    {
        mainCanvas.SetActive(true);
        instructionsCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
        highScoresCanvas.SetActive(false);
    }
}
