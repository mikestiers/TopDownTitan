using UnityEngine;

namespace Playniax.Ignition
{
    [AddComponentMenu("Playniax/Ignition/ObjectCounter")]
    // The ObjectCounter keeps track of the number of GameObjects enabled in the scene.
    //
    // In order for this to work you will need to add the ObjectCounter component to the GameObject that need to be included in the count.
    public class ObjectCounter : MonoBehaviour
    {
        public static int count;
        void OnEnable()
        {
            count += 1;
        }
        void OnDisable()
        {
            count -= 1;
        }
    }
}