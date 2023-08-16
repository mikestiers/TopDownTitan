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

    [Header("Instructions Menu")]
    public GameObject instructionsCanvas;
    public Button instructionsBackButton;

    [Header("Credits Menu")]
    public GameObject creditsCanvas;
    public Button creditsBackButton;

    void Start()
    {
        playButton.onClick.AddListener(Play);
        instructionsButton.onClick.AddListener(InstructionsMenu);
        creditsButton.onClick.AddListener(CreditsMenu);
        instructionsBackButton.onClick.AddListener(BackToMain);
        creditsBackButton.onClick.AddListener(BackToMain);
    }

    void Play()
    {
        SceneManager.LoadScene("Game");
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

    void BackToMain()
    {
        mainCanvas.SetActive(true);
        instructionsCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
    }
}
