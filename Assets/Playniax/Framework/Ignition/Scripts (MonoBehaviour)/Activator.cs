using UnityEngine;

namespace Playniax.Ignition
{
    [AddComponentMenu("Playniax/Ignition/Activator")]
    // Enables the GameObject using a timer.
    //
    // Object can be a GameObject or MonoBehaviour based component.
    public class Activator : MonoBehaviour
    {
        public Object obj;
        public float timer = 3;

        public void Init()
        {
            var gameObject = obj as GameObject;
            var component = obj as MonoBehaviour;

            if (gameObject && gameObject.scene.rootCount > 0) gameObject.SetActive(false);
            else if (component) component.enabled = false;
        }

        void Awake()
        {
            Init();
        }
        void Update()
        {
            timer -= 1 * Time.deltaTime;

            if (timer <= 0)
            {
                timer = 0;

                var gameObject = obj as GameObject;
                var component = obj as MonoBehaviour;

                if (gameObject && gameObject.scene.rootCount > 0)
                {
                    gameObject.SetActive(true);

                    Destroy(this.gameObject);
                }
                else if (component)
                {
                    component.enabled = true;

                    Destroy(this.gameObject);
                }
            }
        }
    }
}
