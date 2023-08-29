using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScreenFlash : MonoBehaviour
{
    public Image whiteScreen;
    public float flashSpeed = 1.0f;
    public string nextSceneName;
    public AudioSource audioSource;
    public AudioClip flashSound;
    private bool isFlashing = false;

    public void OnButtonClick()
    {
        if (!isFlashing)
        {
            StartCoroutine(FlashScreen());
        }
    }

    IEnumerator FlashScreen()
    {
        isFlashing = true;

        for (int i = 0; i < 2; i++)
        {
            float alpha = 0;

            // Play flash sound
            audioSource.PlayOneShot(flashSound);

            // Flash in
            while (alpha < 1)
            {
                alpha += Time.deltaTime * flashSpeed;
                whiteScreen.color = new Color(1, 1, 1, alpha);
                yield return null;
            }

            // Flash out
            while (alpha > 0)
            {
                alpha -= Time.deltaTime * flashSpeed;
                whiteScreen.color = new Color(1, 1, 1, alpha);
                yield return null;
            }
        }

        // Load the next scene after the flash
        SceneManager.LoadScene(nextSceneName);
    }

}
