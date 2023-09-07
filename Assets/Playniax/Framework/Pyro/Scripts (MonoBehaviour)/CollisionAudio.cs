using System.Collections.Generic;
using UnityEngine;
using Playniax.Ignition;

namespace Playniax.Pyro
{
    [AddComponentMenu("Playniax/Pyro/CollisionAudio")]
    // Determines what sound to play when 2 objects collide.
    public class CollisionAudio : MonoBehaviour
    {
        [Tooltip("Material 1.")]
        public string material1 = "Metal";
        [Tooltip("Material 2.")]
        public string material2 = "Metal";
        [Tooltip("Sound to play.")]
        public AudioProperties audioProperties;

        public static void Play(string material1, string material2)
        {
            if (_collisionSound == null) return;

            for (int i = 0; i < _collisionSound.Count; i++)
            {
                if (_collisionSound[i].material1 != "" && _collisionSound[i].material2 != "" && _collisionSound[i].material1 + _collisionSound[i].material2 == material1 + material2 || _collisionSound[i].material1 + _collisionSound[i].material2 == material2 + material1) _collisionSound[i].audioProperties.Play();
            }
        }

        void OnDisable()
        {
            _collisionSound.Remove(this);
        }
        void OnEnable()
        {
            _collisionSound.Add(this);
        }

        static List<CollisionAudio> _collisionSound = new List<CollisionAudio>();
    }
}