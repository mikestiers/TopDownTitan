using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Playniax.Ignition;

namespace Playniax.UI.SimpleGameUI
{
    public class TimerUI : MonoBehaviour
    {
        public int format;
        public string prefix;
        public string suffix;
        public float time = 60;
        public int targetTime = 0;
        public Text text;

        public UnityEvent onTime;

        void Awake()
        {
            if (text == null) text = GetComponent<Text>();
        }

        void Update()
        {
            if ((int)time == targetTime) return;

            if (SimpleGameUI.instance && SimpleGameUI.instance.isBusy == false || SimpleGameUI.instance == null)
            {
                if (time > targetTime)
                {
                    time -= 1 * Time.deltaTime;
                }
                else if (time < targetTime)
                {
                    time += 1 * Time.deltaTime;
                }
            }

            if ((int)time == targetTime)
            {
                onTime.Invoke();
            }

            var count = ((int)time).ToString();

            if (format > 0) count = StringHelpers.Format(count, format);

            if (prefix != "") count = prefix + count;
            if (suffix != "") count += suffix;

            text.text = count;
        }
    }
}