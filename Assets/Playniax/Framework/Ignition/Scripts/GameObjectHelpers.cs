using UnityEngine;

namespace Playniax.Ignition
{

    public class GameObjectHelpers
    {
        public static void SetActiveChildren(GameObject parent, bool value)
        {
            var children = parent.transform.childCount;

            for (int i = 0; i < children; ++i)
                parent.transform.GetChild(i).gameObject.SetActive(value);

        }
    }
}