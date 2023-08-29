using UnityEngine;
using Playniax.Ignition;
using Playniax.ParticleSystem;

namespace Playniax.Pyro
{
    public class DoDamageAndSelfDestruct : CollisionBase2D
    {
        [System.Serializable]
        // Outro Settings determine what effect to play when an object is destroyed.
        public class OutroSettings
        {
            // Determines what emitter to call.
            public string emitterId = "Explosion Red";
            // Determines to emitter scale.
            public float emitterScale = 1;
            // Audio Settings.
            public AudioProperties audioSettings;
            // Determines if outro is used.
            public bool enabled = true;
        }

        public int playerIndex = -1;
        public int damage = 100;
        public OutroSettings outroSettings;
        public override void OnCollision(CollisionBase2D collision)
        {
            if (isSafe == false) return;
            if (collision.isSafe == false) return;

            _UpdateState(collision);

            if (outroSettings.enabled)
            {
                var group = ServiceBase.Get(outroSettings.emitterId) as EmitterGroup;
                if (group == null) return;

                var scale = outroSettings.emitterScale;

                var spriteRenderer = GetComponent<SpriteRenderer>();

                if (spriteRenderer)
                {
                    if (group.size > 0) scale *= Mathf.Max(spriteRenderer.sprite.rect.size.x, spriteRenderer.sprite.rect.size.y) / group.size;

                    scale *= Mathf.Max(transform.localScale.x, transform.localScale.y);

                    group.Call(transform.position, transform.parent, scale, spriteRenderer.sortingOrder);
                }
                else
                {
                    group.Call(transform.position, transform.parent, scale);
                }

                outroSettings.audioSettings.Play();
            }

            Destroy(gameObject);
        }

        void _UpdateState(CollisionBase2D collision)
        {
            var collisionState = collision as CollisionState;

            if (collisionState == null) return;

            collisionState.DoDamage(damage);

            if (collisionState.structuralIntegrity > 0) collisionState.Ghost();

            if (playerIndex > -1 && collisionState.structuralIntegrity == 0)
            {
                PlayerData.Get(playerIndex).scoreboard += collisionState.points;

                CollisionState.OutroSettings.MessengerSettings.Message(collisionState);
            }
        }
    }
}