using UnityEngine;

namespace Playniax.Ignition
{
    namespace Playniax.Ignition
    {
        [AddComponentMenu("Playniax/Ignition/DontDestroyOnLoad")]
        // DontDestroyOnLoad prevents the GameObject from being destroyed when a new scene is loaded.
        public class DontDestroyOnLoad : MonoBehaviour
        {
            public bool dontDestroyOnLoad = true;

            void Awake()
            {
                if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
            }
        }
    }
}