using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Playniax.Ignition;

namespace Playniax.ParticleSystem
{
    [AddComponentMenu("Playniax/Prototyping/Particle System/Emitter Player")]
    public class EmitterPlayer : MonoBehaviour
    {
        [Tooltip("The Identifier.")]
        public string id = "Explosion Red";
        [Tooltip("The scale at which time passes.")]
        public float timeScale = 1;
        // Task scale.
        public float scale = 1;
        [Tooltip("The renderer's order within a sorting layer.")]
        public int orderInLayer;
        [Tooltip("The parent.")]
        public Transform parent;
        [Tooltip("The timer.")]
        public Timer timer;

        public bool mouseMode;

        void LateUpdate()
        {
            if (mouseMode)
            {
                if (Input.GetMouseButton(0))
                {
                    if (timer.Update(true))
                    {
                        var position = Vector3.zero;

                        var mousePosition = Input.mousePosition;

                        mousePosition.z = -Camera.main.transform.position.z;

                        position = Camera.main.ScreenToWorldPoint(mousePosition);

                        var group = EmitterGroup.Call(id, position, parent, scale, orderInLayer, timeScale);

                        if (group == null) Emitter.Play(id, position, parent, scale, orderInLayer, timeScale);
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    timer.timer = 0;
                }
            }
            else
            {
                if (timer.Update())
                {
                    var group = EmitterGroup.Call(id, transform.localPosition, parent, scale, orderInLayer, timeScale);

                    if (group == null) Emitter.Play(id, transform.localPosition, parent, scale, orderInLayer, timeScale);

                    if (timer.counterReachedZero) Destroy(gameObject);
                }
            }
        }
    }
}