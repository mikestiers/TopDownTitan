using System.Linq;
using UnityEngine;
using Playniax.Ignition;
using Playniax.Pyro;

namespace Playniax.InfiniteScroller
{
    [AddComponentMenu("Playniax/Prototyping/Infinite Scroller/Layer Renderer")]
    public class LayerRenderer : MonoBehaviour, IBoundsHelpers
    {
        [Tooltip("The renderer's layer.")]
        public ObjectsLayer layer;

        public Bounds bounds
        {
            get
            {
                var bounds = RendererHelpers.GetBounds(gameObject);

                var size = bounds.size;

                if (this.size.x != 0) size.x = this.size.x;
                if (this.size.y != 0) size.y = this.size.y;
                if (this.size.z != 0) size.z = this.size.z;

                bounds.size = size;

                return bounds;
            }
        }

        public Vector3 size
        {
            get { return _size; }
            set { _size = value; }
        }

#if UNITY_EDITOR
        [Tooltip("Whether to show the gizmos or not.")]
        public bool showGizmos = true;

        void OnDrawGizmos()
        {
            if (layer == null || layer.showGizmos == false || showGizmos == false || enabled == false) return;

            Gizmos.color = layer.gizmoColor;

            Gizmos.DrawWireCube(transform.position, new Vector3(bounds.size.x, bounds.size.y, 1));
        }
        void Reset()
        {
            layer = GetComponent<ObjectsLayer>();

            if (layer == null && transform.parent) layer = transform.parent.GetComponentInParent<ObjectsLayer>();
        }
#endif
        void Awake()
        {
            if (layer == null) layer = GetComponent<ObjectsLayer>();

            if (layer == null && transform.parent) layer = transform.parent.GetComponentInParent<ObjectsLayer>();
        }
        void Start()
        {
            if (layer && layer.objects.Contains(gameObject) == false) layer.objects.Add(gameObject);
        }

        void OnDisable()
        {
            if (layer) layer.objects.Remove(gameObject);
        }

        [SerializeField] Vector3 _size;
    }
}