using UnityEngine;

namespace Playniax.Ignition
{
    [AddComponentMenu("Playniax/Ignition/SelfDeactivator")]
    // Disables the GameObject as soon as it's enabled.
    //
    // This can be useful when the GameObject in the scene is a prefab that isn't actually a Unity prefab.
    public class SelfDeactivator : MonoBehaviour
    {
        void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}
