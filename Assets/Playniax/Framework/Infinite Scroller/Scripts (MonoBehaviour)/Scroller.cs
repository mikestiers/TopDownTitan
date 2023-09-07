using UnityEngine;

namespace Playniax.InfiniteScroller
{
    [AddComponentMenu("Playniax/Prototyping/Infinite Scroller/Scroller")]
    // Infinite Scroller Main Class.
    public class Scroller : MonoBehaviour
    {
        [Tooltip("Determines the world size.")]
        public Vector3 size = new Vector3(100, 100, 0);
        [Tooltip("Scroll speed. Increasing this value will make the 'camera' scroll.")]
        public Vector2 velocity;
        [Tooltip("Scroll friction.")]
        public float friction;
#if UNITY_EDITOR
        [Tooltip("Gizmos color settings.")]
        public Color gizmoColor = new Color(1, 0, 1, 0.5f);
        [Tooltip("Whether to show the gizmos or not.")]
        public bool showGizmos = true;
        void OnDrawGizmos()
        {
            if (showGizmos == false) return;

            Gizmos.color = gizmoColor;

            Gizmos.DrawWireCube(transform.position, new Vector3(size.x, size.y, 1));
        }
#endif
        // Returns an array of all layers.
        public LayerBase[] GetLayers()
        {
            //if (layers.Length > 0) return layers;

            return GetComponentsInChildren<LayerBase>();
        }

        // Moves the objects in the direction.
        public void Translate(float x, float y)
        {
            for (int i = 0; i < GetLayers().Length; i++)
            {
                GetLayers()[i].Scroll(x, y);
            }
        }

        void Awake()
        {
            //if (layers.Length == 0) layers = GetComponentsInChildren<LayerBase>();
        }

        void LateUpdate()
        {
            if (velocity == default)
            {
                Translate(0, 0);
            }
            else
            {
                Translate(velocity.x * Time.deltaTime, velocity.y * Time.deltaTime);
            }

            if (friction != 0) velocity *= 1 / (1 + (Time.deltaTime * friction));
        }
    }
}