using UnityEngine;

namespace Playniax.UI.SimpleGameUI
{
    public class MusicPlayer : MonoBehaviour
    {
        public AudioClip audioClip;
        public AudioSource audioSource;
        public bool loop = true;

        void Start()
        {
            if (SimpleGameUI.instance) audioSource = SimpleGameUI.instance.musicSettings.audioSource;

            if (audioSource == null) audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.loop = loop;
            }

            if (audioSource && audioClip)
            {
                audioSource.clip = audioClip;

                if (PlayerPrefs.GetInt("musicOff") == 0) audioSource.Play();
            }
        }
    }
}