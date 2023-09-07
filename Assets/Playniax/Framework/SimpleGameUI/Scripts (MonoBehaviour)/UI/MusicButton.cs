using UnityEngine;

namespace Playniax.UI.SimpleGameUI
{
    public class MusicButton : Rotator
    {
        public override void OnClick(int state)
        {
            if (SimpleGameUI.instance)
            {
                PlayerPrefs.SetInt("musicOff", state);

                SimpleGameUI.instance.musicSettings.SetPause(state != 0);
            }
        }

        public void OnEnable()
        {
            if (SimpleGameUI.instance)
            {
                state = PlayerPrefs.GetInt("musicOff");

                if (states[state]) targetGraphic.sprite = states[state];
            }
        }
    }
}