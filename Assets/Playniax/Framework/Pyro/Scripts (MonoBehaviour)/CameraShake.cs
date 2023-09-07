using System.Collections;
using UnityEngine;

namespace Playniax.Pyro
{
    // https://youtu.be/BQGTdRhGmE4

    public class CameraShake : MonoBehaviour
    {
        public float duration = 1;
        public Vector3 range = new Vector3(1, 1, 1);
        public AnimationCurve curve;
        public void Shake()
        {
            if (_coroutine == null) _coroutine = StartCoroutine(_Shake());
        }

        IEnumerator _Shake()
        {
            var position = Camera.main.transform.position;

            float timer = 0f;

            while (timer < duration)
            {
                var shake = Vector3.Scale(Random.insideUnitSphere, range);

                if (curve.length > 0) shake *= curve.Evaluate(timer / duration);

                Camera.main.transform.position = position + shake;

                timer += Time.deltaTime;

                yield return null;
            }

            Camera.main.transform.position = position;

            _coroutine = null;
        }

        Coroutine _coroutine;
    }
}