using UnityEngine;
using Playniax.Ignition;

public class AudioChannels_Example : MonoBehaviour
{
    // Ignition sound object.
    public AudioProperties audioProperties;

    void Start()
    {
        // State to console.
        Debug.Log(AudioChannels.mute ? "off" : "on");
    }
    public void Play()
    {
        // Play sound
        audioProperties.Play();
    }

    public void Mute()
    {
        // Toggle sound
        AudioChannels.mute = !AudioChannels.mute;

        // State to console.
        Debug.Log(AudioChannels.mute ? "off" : "on");
    }
}
