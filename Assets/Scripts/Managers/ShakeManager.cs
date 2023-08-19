using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShakeManager : MonoBehaviour
{
    public float shakeDetectionThreshold = 2.0f;
    public float shakeInterval = 0.5f;
    private float timeSinceLastShake = 0.0f;

    public GameObject pauseMenu;

    void Update()
    {
        timeSinceLastShake += Time.deltaTime;

        // Check if enough time has passed since the last shake
        if (timeSinceLastShake > shakeInterval)
        {
            // Calculate the device's acceleration
            Vector3 acceleration = Input.acceleration;

            // Calculate the magnitude of the acceleration
            float accelerationMagnitude = acceleration.magnitude;

            // Check if the acceleration magnitude exceeds the shake detection threshold
            if (accelerationMagnitude > shakeDetectionThreshold)
            {
                // A shake has been detected, trigger the pause menu
                PauseGame();

                // Reset the time since the last shake
                timeSinceLastShake = 0.0f;
            }
        }
    }

    void PauseGame()
    {
        HUD.singleton.Pause();
    }
}
