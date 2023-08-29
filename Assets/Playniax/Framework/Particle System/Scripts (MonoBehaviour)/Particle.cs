#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections.Generic;
using UnityEngine;
using Playniax.Ignition;

namespace Playniax.ParticleSystem
{
    public class Particle : MonoBehaviour
    {
        [Tooltip("The scale at which time passes.")]
        public float timeScale = 1;
        [Tooltip("Suspends the execution for the given amount.")]
        public float delay;

        public float ttl
        {
            get
            {
                return _ttl;
            }
            set
            {
                _ttlTimer = value;

                _ttl = value;
            }
        }

        [Tooltip("The velocity of the particle.")]
        public Vector3 velocity;
        [Tooltip("The constant of the particle.")]
        public Vector3 constant;
        [Tooltip("The friction of the particle")]
        public float friction;
        [Tooltip("The gravity of the particle.")]
        public float gravity;
        [Tooltip("The spinning speed of the particle.")]
        public Vector3 spin;
        public Color startColor
        {
            get
            {
                return _startColor;
            }
            set
            {
                var spriteRenderer = GetSpriteRenderer();
                if (spriteRenderer) spriteRenderer.color = value;

                _startColor = value;
            }
        }

        public Color targetColor;
        public Vector3 startScale
        {
            get
            {
                return _startScale;
            }
            set
            {
                transform.localScale = value;

                _startScale = value;
            }
        }

        [Tooltip("The target scale of the particle.")]
        public Vector3 targetScale;

        public SpriteRenderer GetSpriteRenderer()
        {
            if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();

            return _spriteRenderer;
        }

        void OnEnable()
        {
            _ttlTimer = ttl;
        }
        void LateUpdate()
        {
            UpdateParticle();
        }

        float _GetDeltaTime()
        {
            return Time.deltaTime * timeScale;
        }

        public void  DestroyParticle()
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                EditorApplication.update -= UpdateParticle;

                DestroyImmediate(gameObject);
            }
            else
            {
                RuntimeDestroy();
            }
#else
                RuntimeDestroy();
#endif
            void RuntimeDestroy()
            {
                if (name.Contains(ObjectPooler.MARKER))
                {
                    transform.parent = null;

                    gameObject.SetActive(false);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

        public void UpdateParticle()
        {
            if (gameObject == null) return;

            var spriteRenderer = GetSpriteRenderer();
            if (spriteRenderer == null) return;

            delay -= 1 * _GetDeltaTime();

            if (delay > 0) return;

            if (spriteRenderer.enabled == false) spriteRenderer.enabled = true;

            transform.localPosition += constant * _GetDeltaTime();
            transform.localPosition += velocity * _GetDeltaTime();

            transform.Rotate(spin * _GetDeltaTime());

            if (friction != 0) velocity *= 1 / (1 + (_GetDeltaTime() * friction));

            velocity.y -= gravity * _GetDeltaTime();

            if (ttl > 0 && _ttlTimer > 0)
            {
                spriteRenderer.color = targetColor - (targetColor - startColor) * (_ttlTimer / ttl);
                transform.localScale = targetScale - (targetScale - startScale) * (_ttlTimer / ttl);

                _ttlTimer -= 1 * _GetDeltaTime();
            }
            else
            {
                DestroyParticle();
            }
        }

        SpriteRenderer _spriteRenderer;
        float _ttlTimer;

        [SerializeField]
        Color _startColor = Color.white;

        [SerializeField]
        Vector3 _startScale = Vector3.one;

        [SerializeField]
        float _ttl = 1;
    }
}