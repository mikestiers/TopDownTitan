using UnityEngine;

namespace Playniax.InfiniteScroller
{
    [AddComponentMenu("Playniax/Prototyping/Infinite Scroller/Objects Layer")]
    public class ObjectsLayer : LayerBase
    {
        public Scroller scroller;

#if UNITY_EDITOR
        [Tooltip("Gizmos color settings.")]
        public Color gizmoColor = new Color(1, 0, 0, 0.5f);
        [Tooltip("Whether to show the gizmos or not.")]
        public bool showGizmos = true;
        void OnDrawGizmos()
        {
            if (showGizmos == false) return;

            if (scroller == null) return;

            var layers = scroller.GetComponentsInChildren<LayerBase>();

            if (layers == null) return;

            if (layers.Length == 0) return;

            var lowest = layers[0].speed;

            for (int i = 0; i < layers.Length; i++)
            {
                if (layers[i].speed < lowest) lowest = layers[i].speed;
            }

            Gizmos.color = gizmoColor;

            Gizmos.DrawWireCube(transform.position, new Vector3(scroller.size.x * speed / lowest, scroller.size.y * speed / lowest, 1));
        }
        void Reset()
        {
            scroller = FindObjectOfType<Scroller>();
        }
#endif
        public override void Scroll(float x, float y)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] == null) continue;

                var size = _GetScrollEdge() * speed * .5f;

                var position = objects[i].transform.position;

                position.x -= x * speed;
                position.y -= y * speed;

                if (position.x > size.x) position.x -= size.x * 2;
                if (position.x < -size.x) position.x += size.x * 2;

                if (position.y > size.y) position.y -= size.y * 2;
                if (position.y < -size.y) position.y += size.y * 2;

                objects[i].transform.position = position;
            }
        }

        void Awake()
        {
            if (scroller == null) scroller = FindObjectOfType<Scroller>();
        }

        Vector2 _GetScrollEdge()
        {
            var layers = scroller.GetLayers();

            var speed = layers[0].speed;

            for (int i = 0; i < layers.Length; i++)
            {
                if (layers[i].speed < speed) speed = layers[i].speed;
            }

            return scroller.size / speed;
        }
    }
}