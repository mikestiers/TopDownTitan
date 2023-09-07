using UnityEngine;
using UnityEngine.UI;
using Playniax.Ignition;

namespace Playniax.Ignition.UI
{
    public class CoinsUI : MonoBehaviour
    {
        public int playerIndex;
        public int format = 8;
        public string prefix;
        public string suffix;
        public Text text;
        void Awake()
        {
            if (text == null) text = GetComponent<Text>();
        }
        void Update()
        {
            var count = PlayerData.Get(playerIndex).coins.ToString();

            if (format > 0) count = StringHelpers.Format(count, format);

            if (prefix != "") count = prefix + count;
            if (suffix != "") count += suffix;

            text.text = count;
        }
    }
}