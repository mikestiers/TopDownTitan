using UnityEngine;
using Playniax.UI.SimpleGameUI;

namespace Playniax.Ads
{
    public class TestIntermission : MonoBehaviour
    {
        public float timer = 5;

        void Update()
        {
            if (timer <= 0)
            {
                enabled = false;
                if (SimpleGameUI.instance) SimpleGameUI.instance.Intermission(false);
                return;
            }

            timer -= Time.deltaTime;
        }
    }
}

