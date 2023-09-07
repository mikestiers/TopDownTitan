using UnityEngine;

namespace Playniax.Ignition
{
    public class Deactivator : MonoBehaviour
    {
        public GameObject[] list;

        void Awake()
        {
            foreach (GameObject gameObject in list)
                if (gameObject.scene.rootCount > 0) gameObject.SetActive(false);
        }
    }
}