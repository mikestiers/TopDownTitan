using UnityEngine;

namespace Playniax.Ignition
{
    public class Debugging
    {
        public static void GetTime()
        {
            if (_time == -1) _time = Time.deltaTime;

            var time = Time.deltaTime - _time;

            _time = -1;

            Debug.Log(time);
        }

        static float _time = -1;
    }
}
