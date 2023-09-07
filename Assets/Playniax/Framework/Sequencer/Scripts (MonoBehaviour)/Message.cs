using UnityEngine;
using Playniax.Pyro;
using Playniax.UI.SimpleGameUI;

namespace Playniax.Sequencer
{
    public class Message : SequenceBase
    {
        public string messengerId = "Generic";
        [Multiline]
        [Tooltip("The message to display.")]
        public string text = "Get Ready For Wave %WAVE%";
        [Tooltip("Time to wait before fading")]
        public float sustain = 1;
        [Tooltip("Fading display time")]
        public float fadeTime = 1.5f;
        public float scaleStep;
        [Tooltip("Font")]
        public Font font;
        [Tooltip("Font size")]
        public int fontSize = 32;
        [Tooltip("Font color")]
        public Color fontColor = Color.white;
        public bool dontWait = true;
        public bool useFallBackDisplay = true;
        public override void OnSequencerUpdate()
        {
            if (state == 1)
            {
                var display = _Fetch(text);

                if (sequencer && display.Contains("%WAVE%"))
                {
                    display = display.Replace("%WAVE%", sequencer.wave.ToString());

                    sequencer.wave += 1;
                }

                if (SimpleGameUI.instance)
                {
                    SimpleGameUI.instance.screenSettings.effects.Message(font, fontSize, fontColor, display, sustain, scaleStep);
                }
                else if (useFallBackDisplay && Messenger.instance)
                {
                    Messenger.instance.Create(messengerId, display, Vector3.zero);
                }
                else if (useFallBackDisplay && TextEffect.current == null)
                {
                    TextEffect.current = TextEffect.Create(display, Vector3.zero, font, fontSize, sustain, fadeTime);
                }

                if (dontWait == true) enabled = false; else state = 2;
            }
            else if (state == 2)
            {
                if (SimpleGameUI.instance)
                {
                    if (SimpleGameUI.instance.isMessengerBusy == false) enabled = false;
                }
                else if (useFallBackDisplay && Messenger.instance)
                {
                    if (Messenger.instance.queue.Count == 0) enabled = false;
                }
                else if (useFallBackDisplay && TextEffect.current == null)
                {
                    if (TextEffect.current == null) enabled = false;
                }
            }
        }

        string _Fetch(string text)
        {
            if (SimpleGameUI.instance)
            {
                text = text.Replace("%LEVEL%", SimpleGameUI.instance.GetCurrentLevel().ToString());
            }
            else
            {
                text = text.Replace("%LEVEL%", "0");
            }

            return text;
        }
    }
}