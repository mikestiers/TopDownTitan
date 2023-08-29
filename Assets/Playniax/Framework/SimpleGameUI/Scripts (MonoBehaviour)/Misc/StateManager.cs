using System;
using UnityEngine;
using Playniax.Ignition;

namespace Playniax.UI.SimpleGameUI
{
    public class StateManager : MonoBehaviour
    {
        [System.Serializable]
        public class Base
        {
            public float delay = .5f;
            public float sustain = .75f;
            public float scaleStep = .025f;
            public Font font;
            public int fontSize = 48;
            public Color color = Color.white;
        }

        [System.Serializable]
        public class GameCompletedSettings : Base
        {
            [Multiline]
            public string text = "CONGRTULATIONS!,GAME COMPLETED!";
        }

        [System.Serializable]
        public class IntroSettings : Base
        {
            [Multiline]
            public string text = "GET READY FOR\nLEVEL %LEVEL%";
        }

        [System.Serializable]
        public class LevelCompletedSettings : Base
        {
            [Multiline]
            public string text = "LEVEL %LEVEL%\nCOMPLETED";
        }

        [System.Serializable]
        public class ReplaySettings : Base
        {
            [Multiline]
            public string text = "FAIL!,TRY AGAIN!";
        }

        [System.Serializable]
        public class GameOverSettings
        {
            public float delay = 1.5f;
        }

        public IntroSettings introSettings;
        public LevelCompletedSettings levelCompletedSettings;
        public GameCompletedSettings gameCompletedSettings;
        public ReplaySettings replaySettings;
        public GameOverSettings gameOverSettings;
        public static StateManager instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<StateManager>();

                return _instance;
            }
        }
        public bool isMessengerBusy
        {
            get
            {
                if (_messengerState != 0) return true; else return false;
            }
        }
        public virtual void OnGameCompleted()
        {
            if (SimpleGameUI.instance) _Message(gameCompletedSettings, gameCompletedSettings.delay, gameCompletedSettings.text, SimpleGameUI.instance.LevelUp);
        }

        public virtual void OnGameOver()
        {
            if (SimpleGameUI.instance) SimpleGameUI.instance.GameOver(gameOverSettings.delay);
        }

        public virtual void OnOneDown()
        {
            if (SimpleGameUI.instance) _Message(replaySettings, replaySettings.delay, replaySettings.text, SimpleGameUI.instance.Reload);
        }

        public virtual void OnLevelCompleted()
        {
            if (SimpleGameUI.instance) _Message(levelCompletedSettings, levelCompletedSettings.delay, levelCompletedSettings.text, SimpleGameUI.instance.LevelUp);
        }

        public virtual bool isGameOver
        {
            get
            {
                if (PlayersGroup.GetList().Count == 0 && PlayerData.CountLives() == 0 && SimpleGameUI.instance != null && SimpleGameUI.instance.screenSettings.gameOver && SimpleGameUI.instance.screenSettings.gameOver.isActiveAndEnabled == false && SimpleGameUI.instance.screenSettings.inGame.isActiveAndEnabled == true) return true;

                return false;
            }
        }

        public virtual bool isKilled
        {
            get
            {
                if (PlayersGroup.GetList().Count == 0 && PlayerData.CountLives() > 0) return true;

                return false;
            }
        }

        public virtual bool isLastLevel
        {
            get
            {
                if (SimpleGameUI.instance != null) return SimpleGameUI.instance.isLastLevel;

                return false;
            }
        }

        public virtual bool isLevelCompleted
        {
            get
            {
                if (SimpleGameUI.instance != null) return SimpleGameUI.instance.isLevelCompleted;

                return false;
            }
        }

        void Start()
        {
            if (SimpleGameUI.instance)
            {
                gameCompletedSettings.text = _Fetch(gameCompletedSettings.text);
                introSettings.text = _Fetch(introSettings.text);
                levelCompletedSettings.text = _Fetch(levelCompletedSettings.text);

                if (SimpleGameUI.instance.screenSettings.effects.messenger.isActiveAndEnabled == false)
                {
                    _Message(introSettings, introSettings.delay, introSettings.text);
                }
            }
        }

        void Update()
        {
#if UNITY_EDITOR
            _TestKeys();
#endif
            _UpdateMonitor();
            _UpdateMessenger();
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

        void _Message(Base mode, float delay, string message, Action OnMessageDone = null)
        {
            if (SimpleGameUI.instance)
            {
                SimpleGameUI.instance.screenSettings.effects.messenger.gameObject.SetActive(false);

                _index = 0;
                _messengerState = 0;

                _mode = mode;
                _timer = delay;
                _sequence = message.Split(","[0]);
                _onMessageDone = OnMessageDone;
            }
        }

        void _UpdateMonitor()
        {
            if (_monitoringSuspended == false)
            {
                if (isGameOver == true)
                {
                    OnGameOver();

                    _monitoringSuspended = true;
                }
                else if (isKilled == true)
                {
                    OnOneDown();

                    _monitoringSuspended = true;
                }
                else if (isLevelCompleted)
                {
                    if (isLastLevel)
                    {
                        OnGameCompleted();

                        _monitoringSuspended = true;
                    }
                    else
                    {
                        OnLevelCompleted();

                        _monitoringSuspended = true;
                    }
                }
            }
        }

        void _TestKeys()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                var settings = levelCompletedSettings;

                _Message(settings, settings.delay, settings.text, SimpleGameUI.instance.LevelUp);
            }
        }

        void _UpdateMessenger()
        {
            if (_messengerState == 0 && SimpleGameUI.instance && SimpleGameUI.instance.screenSettings.effects.messenger.isActiveAndEnabled == false && _mode != null && _sequence != null && _sequence.Length > 0)
            {
                if (_timer > 0)
                {
                    _timer -= 1 * Time.deltaTime;
                }
                else
                {
                    SimpleGameUI.instance.screenSettings.effects.Message(SimpleGameUI.GetFont(_mode.font), _mode.fontSize, _mode.color, _sequence[_index], _mode.sustain, _mode.scaleStep);

                    _index += 1;

                    if (_index >= _sequence.Length)
                    {
                        _index = 0;
                        _mode = null;
                        _timer = 0;
                        _sequence = null;

                        _messengerState = 1;
                    }
                }
            }

            if (_messengerState == 1 && SimpleGameUI.instance && SimpleGameUI.instance.screenSettings.effects.messenger.isActiveAndEnabled == false)
            {
                if (_onMessageDone != null)
                {
                    _onMessageDone();
                    _onMessageDone = null;
                }

                _messengerState = 0;
            }
        }

        static StateManager _instance;

        int _index;
        int _messengerState;
        Base _mode;
        bool _monitoringSuspended;
        Action _onMessageDone;
        string[] _sequence;
        float _timer;
    }
}
