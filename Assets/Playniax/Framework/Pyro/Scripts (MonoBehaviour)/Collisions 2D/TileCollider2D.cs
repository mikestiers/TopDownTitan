using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace Playniax.Pyro
{
    public class TileCollider2D : MonoBehaviour
    {
        public TilemapCollider2D tilemapCollider;
        public Collider2D[] colliders;
        public UnityEvent onCollision;

        void Start()
        {
            if (colliders.Length == 0) colliders = GetComponentsInChildren<Collider2D>();
        }

        void Update()
        {
            if (tilemapCollider == null) return;

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] == null) continue;

                if (tilemapCollider.isActiveAndEnabled == false) continue;
                if (colliders[i].isActiveAndEnabled == false) continue;
                if (tilemapCollider == colliders[i]) continue;

                if (tilemapCollider.Distance(colliders[i]).isOverlapped)
                {
                    onCollision.Invoke();

                    break;
                }
            }
            {
                //collisionState.Kill();
            }
        }
    }
}
