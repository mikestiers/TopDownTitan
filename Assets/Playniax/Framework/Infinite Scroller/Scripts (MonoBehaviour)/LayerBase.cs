using System.Collections.Generic;
using UnityEngine;

namespace Playniax.InfiniteScroller
{
    // Base class for layers.
    public class LayerBase : MonoBehaviour
    {
        [Tooltip("Scroll speed or depth.")]
        public float speed = 1;
        [Tooltip("Objects attached to the layer.")]
        public List<GameObject> objects;
        public virtual void Scroll(float x, float y)
        {
        }
    }
}