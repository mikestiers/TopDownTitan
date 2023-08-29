using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Playniax.Ignition;
using System.Collections.Generic;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

namespace Playniax.UI.SimpleGameUI
{
    // The SimpleGameUI will manage the different screens and can hold game data that has to be maintained throughout the game session like number of lives, current level, purchases, etc.
    public class SimpleGameUI : MonoBehaviour
    {
        [System.Serializable]
        public class MusicSettings
        {
            public AudioSource audioSource;
            public void Init(MonoBehaviour parent)
            {
                if (parent && audioSource == null) audioSource = parent.GetComponent<AudioSource>();
                if (audioSource == null) audioSource = parent.gameObject.AddComponent<AudioSource>();
            }
            public void Play()
            {
                if (audioSource && PlayerPrefs.GetInt("musicOff") == 0) audioSource.Play();
            }
            public void SetPause(bool value)
            {
                if (value == true)
                {
                    if (audioSource) audioSource.Pause();
                }
                else
                {
                    if (PlayerPrefs.GetInt("musicOff") == 0)
                    {
                        Play();

                        if (audioSource) audioSource.UnPause();
                    }
                }
            }
            public void Stop()
            {
                if (audioSource) audioSource.Stop();
            }
        }

        [System.Serializable]
        public class PlayerSettings
        {
            public int lives = 3;

            //[TextArea(5, 10)]
            //public string defaults;
        }

        [System.Serializable]
        public class ScreenSettings
        {
            public bool pauseAsMain;
            public bool loadingProgress = true;

            [Header("Screens")]
            public Home home;
            public InGame inGame;
            public About about;
            public GameOver gameOver;
            public Pause pause;
            public Settings settings;
            public Shop shop;
            public Loader loader;
            public Effects effects;
            public Reset reset;

            public HideObjectsByName hideObjectsByName;
        }

        [System.Serializable]
        public class HideObjectsByName
        {
            public bool enabled;
            public string hide;

            public void Hide(GameObject parent)
            {
                if (enabled == false) return;

                if (_hide == null) _hide = hide.Split(char.Parse(","));
                if (_hide == null) return;

                foreach (Transform child in parent.transform.GetComponentsInChildren<Transform>())
                    if (_Find(child.name)) child.gameObject.SetActive(false);
            }

            bool _Find(string name)
            {
                for (int i = 0; i < _hide.Length; i++)
                {
                    if (_hide[i] == name) return true;
                }

                return false;
            }

            string[] _hide;
        }
        public IEnumerator Load(Func<string> scene, GameObject showUI = null, bool startPaused = false, bool fade = true, string transition = "", string[] advertisements = null)
        {
            _loading = true;

            musicSettings.Stop();

            if (fade)
            {
                screenSettings.effects.SetScreenFader(1);

                while (screenSettings.effects.screenFaderImage.color.a != screenSettings.effects.screenFaderTarget) yield return null;
            }

            //Timing.Paused = true;

            screenSettings.effects.messenger.gameObject.SetActive(false);

            if (screenSettings.home) screenSettings.home.gameObject.SetActive(false);
            if (screenSettings.inGame) screenSettings.inGame.gameObject.SetActive(false);
            if (screenSettings.about) screenSettings.about.gameObject.SetActive(false);
            if (screenSettings.gameOver) screenSettings.gameOver.gameObject.SetActive(false);
            if (screenSettings.pause) screenSettings.pause.gameObject.SetActive(false);
            if (screenSettings.settings) screenSettings.settings.gameObject.SetActive(false);
            if (screenSettings.shop) screenSettings.shop.gameObject.SetActive(false);
            if (screenSettings.reset) screenSettings.reset.gameObject.SetActive(false);

            screenSettings.effects.pauseFader.gameObject.SetActive(false);

            if (allowTransition && transition != "")
            {
                _intermission = true;

                SceneManager.LoadScene(transition);

                if (fade)
                {
                    screenSettings.effects.SetScreenFader(-1);

                    while (screenSettings.effects.screenFaderImage.color.a != screenSettings.effects.screenFaderTarget)
                    {
                        yield return null;
                    }
                }

                while (_intermission == true) yield return null;

                if (fade)
                {
                    screenSettings.effects.SetScreenFader(1);

                    while (screenSettings.effects.screenFaderImage.color.a != screenSettings.effects.screenFaderTarget) yield return null;
                }
            }

            if (allowAdvertisements && advertisements != null && advertisements.Length > 0 && _intermission == false && PlayerPrefs.GetInt(NO_ADS_PRODUCT_ID) == 0)
            {
                while (advertisements.Length > 0)
                {
                    _intermission = true;

                    var advertisement = advertisements[0];

                    SceneManager.LoadScene(advertisement);

                    if (fade)
                    {
                        screenSettings.effects.SetScreenFader(-1);

                        while (screenSettings.effects.screenFaderImage.color.a != screenSettings.effects.screenFaderTarget)
                        {
                            yield return null;
                        }
                    }

                    advertisements = ArrayHelpers.Skim(advertisements);

                    while (_intermission == true) yield return null;

                    if (fade)
                    {
                        screenSettings.effects.SetScreenFader(1);

                        while (screenSettings.effects.screenFaderImage.color.a != screenSettings.effects.screenFaderTarget) yield return null;
                    }
                }
            }

            if (screenSettings.loader && screenSettings.loadingProgress) screenSettings.loader.gameObject.SetActive(true);

            asyncOperation = SceneManager.LoadSceneAsync(scene.Invoke());

            asyncOperation.allowSceneActivation = false;

            yield return null;

            while (asyncOperation.isDone == false)
            {
                if (screenSettings.loader)
                {
                    if (screenSettings.loader.progressText) screenSettings.loader.progressText.text = (int)(asyncOperation.progress * (100 / .9f)) + "%";
                    if (screenSettings.loader.progressBar) screenSettings.loader.progressBar.value = (int)(asyncOperation.progress * (100 / .9f));
                }

                if (asyncOperation.progress >= 0.9f)
                {
                    yield return null;

                    asyncOperation.allowSceneActivation = true;

                    //Timing.Paused = true;

                    yield return null;
                }

                yield return null;
            }

            if (screenSettings.loader) screenSettings.loader.gameObject.SetActive(false);

            Timing.Paused = startPaused;

            if (startPaused) screenSettings.effects.SetPauseFader(1);

            if (showUI) showUI.gameObject.SetActive(true);

            if (fade)
            {
                screenSettings.effects.SetScreenFader(-1);

                while (screenSettings.effects.screenFaderImage.color.a != screenSettings.effects.screenFaderTarget)
                {
                    yield return null;
                }
            }

            _loading = false;
        }

        const string NO_ADS_PRODUCT_ID = "noads";

        AsyncOperation asyncOperation;

        // Determines what scene to load when your app starts. backgroundScene will run on the background of the SimpleGameUI. Left blank the SimpleGameUI will load the first level or last level played depending on settings. Don’t forget to add these scenes to the Unity Build Settings or they won’t load!
        public string backgroundScene;
        // Determines the scene starts paused or not (TimeScale = 0).
        public bool startPaused = true;
        // Whether to reset the game or not on every play.
        public bool newGameEverytime = true;
        public MusicSettings musicSettings;
        public PlayerSettings playerSettings;
        // The levels or scenes to load as levels. Don’t forget to add these scenes to the Unity Build Settings or they won’t load!
        public string[] levelSettings;
        // Whether to show the transition scene or not.
        public bool allowTransition = true;
        // The transition scene will be shown after each level is completed.
        public string transition;
        // Whether to show an ad between loading.
        public bool allowAdvertisements = true;
        // The scene(s) containing the ad(s). Don’t forget to add these scenes to the Unity Build Settings or they won’t load!
        public string[] advertisements;
        public ScreenSettings screenSettings;
        // Fallback font.
        public Font font;

#if UNITY_EDITOR
        [Header("Simulation keys (+ Left Shift)")]
        public KeyCode gameOver = KeyCode.G;
        public KeyCode nextLevel = KeyCode.N;
        public KeyCode resetGame = KeyCode.R;
        public KeyCode reloadLevel = KeyCode.L;

        [Space(10)]
        public string[] searchInFolders;
        public bool ignoreSearchInFoldersAndIsolate;
#endif
        // Returns the instance of the SimpleGameUI.
        public static SimpleGameUI instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<SimpleGameUI>();

                return _instance;
            }
        }

        // Returns the fallback font.
        public static Font GetFont(Font font)
        {
            if (instance && instance.font) return instance.font;

            return font;
        }

        // Returns the state of the intermission playing or not.
        public void Intermission(bool state)
        {
            _intermission = state;
        }

        // Resets the game.
        public virtual string ResetGame()
        {
            PlayerData.Reset(playerSettings.lives);

            PlayerPrefs.SetInt("levelIndex", 0);
            PlayerPrefs.Save();

            return levelSettings[0];
        }

        public void Delay(Action action, float time)
        {
            if (_onTimerDone != null) return;

            _onTimerDone = action;
            _timer = time;
        }

        // Whether the SimpleGameUI is showing UI activity or not.
        public bool isBusy
        {
            get
            {
                if (
                    _loading == true ||
                    _onTimerDone != null ||
                    _intermission == true ||
                    //(screenSettings.effects && screenSettings.effects.isActiveAndEnabled == true) ||
                    (screenSettings.home && screenSettings.home.isActiveAndEnabled == true) ||
                    (screenSettings.about && screenSettings.about.isActiveAndEnabled == true) ||
                    (screenSettings.gameOver && screenSettings.gameOver.isActiveAndEnabled == true) ||
                    (screenSettings.pause && screenSettings.pause.isActiveAndEnabled == true) ||
                    (screenSettings.settings && screenSettings.settings.isActiveAndEnabled == true) ||
                    (screenSettings.shop && screenSettings.shop.isActiveAndEnabled == true) ||
                    (screenSettings.loader && screenSettings.loader.isActiveAndEnabled == true) ||
                    (screenSettings.reset && screenSettings.reset.isActiveAndEnabled == true)
                    )
                    return true;

                return false;
            }
        }

        // Whether the level is completed or not. This is still experimental.
        public bool isLevelCompleted
        {
            get
            {
                if (
                    Timing.Paused == false &&
                    asyncOperation != null && asyncOperation.isDone == true &&
                    SpawnerBase.count == 0 &&
                    ObjectCounter.count == 0 &&
                    GameData.progress == 0 &&
                    ((screenSettings.inGame && screenSettings.inGame.isActiveAndEnabled == true) || screenSettings.inGame == null) &&
                    ((screenSettings.home && screenSettings.home.isActiveAndEnabled == false) || screenSettings.home == null) &&
                    ((screenSettings.about && screenSettings.about.isActiveAndEnabled == false) || screenSettings.about == null) &&
                    ((screenSettings.pause && screenSettings.pause.isActiveAndEnabled == false) || screenSettings.pause == null) &&
                    ((screenSettings.settings && screenSettings.settings.isActiveAndEnabled == false) || screenSettings.settings == null) &&
                    ((screenSettings.shop && screenSettings.shop.isActiveAndEnabled == false) || screenSettings.shop == null) &&
                    ((screenSettings.gameOver && screenSettings.gameOver.isActiveAndEnabled == false) || screenSettings.gameOver == null) &&
                    ((screenSettings.loader && screenSettings.loader.isActiveAndEnabled == false) || screenSettings.loader == null) &&
                    ((screenSettings.reset && screenSettings.reset.isActiveAndEnabled == false) || screenSettings.reset == null) &&
                    screenSettings.effects.flash.activeInHierarchy == false &&
                    screenSettings.effects.pauseFader.activeInHierarchy == false &&
                    screenSettings.effects.gameFader.activeInHierarchy == false &&
                    screenSettings.effects.screenFader.activeInHierarchy == false &&
                    screenSettings.effects.messenger.isActiveAndEnabled == false &&
                    _intermission == false &&
                    _loading == false
                    )
                    return true;

                return false;
            }
        }

        // Whether the last level is loaded or not.
        public bool isLastLevel
        {
            get
            {
                var levelIndex = PlayerPrefs.GetInt("levelIndex");
                if (levelIndex == GetLevels() - 1) return true;
                return false;
            }
        }

        // Whether the loading screen is active or not.
        public bool isLoading
        {
            get
            {
                return _loading;
            }
        }

        // Whether the messenger is active or not.
        public bool isMessengerBusy
        {
            get
            {
                return screenSettings.effects.messenger.isActiveAndEnabled;
            }
        }

        // Whether the mouse is hovering the pausebutton or not.
        public bool isMouseOverPauseButton
        {
            get
            {
                if ((screenSettings.inGame && screenSettings.inGame.isActiveAndEnabled == false) || screenSettings.inGame == null) return false;
                if (_pauseRect == null) _pauseRect = screenSettings.inGame.pauseButton.gameObject.GetComponent<RectTransform>();
                var mousePosition = _pauseRect.InverseTransformPoint(Input.mousePosition);
                return _pauseRect.rect.Contains(mousePosition);
            }
        }

        // Is called when player selects the about button.
        public void AboutButton()
        {
            if (screenSettings.about == null) return;

            if (screenSettings.home && screenSettings.home.gameObject.activeInHierarchy) _back = screenSettings.home.gameObject;
            if (screenSettings.pause && screenSettings.pause.gameObject.activeInHierarchy) _back = screenSettings.pause.gameObject;
            if (screenSettings.gameOver && screenSettings.gameOver.gameObject.activeInHierarchy) _back = screenSettings.gameOver.gameObject;

            _back.SetActive(false);

            _current = screenSettings.about.gameObject;

            _current.SetActive(true);
        }

        // Is called when player selects the back button.
        public void BackButton()
        {
            if (_back == null) return;
            if (_current == null) return;

            _current.SetActive(false);

            _back.SetActive(true);

            _back = null;
            _current = null;
        }

        // Is called when player selects the exit button.
        public void ExitButton()
        {
            _Load();
        }

        // Will show the Game Over page. Usually this is managed by the SimpleGameUI or other helpers but there might be circumstances where you want to manage it yourself.
        public void GameOver()
        {
            if (screenSettings.gameOver == null) return;

            screenSettings.effects.messenger.gameObject.SetActive(false);

            if (screenSettings.home) screenSettings.home.gameObject.SetActive(false);
            if (screenSettings.inGame) screenSettings.inGame.gameObject.SetActive(false);
            if (screenSettings.pause) screenSettings.pause.gameObject.SetActive(false);

            screenSettings.gameOver.gameObject.SetActive(true);

            screenSettings.effects.SetPauseFader(1);

            Timing.Paused = true;
        }

        // Will show the Game Over page but after some delay. Usually this is managed by the SimpleGameUI but there might be circumstances where you want to manage it yourself.
        public void GameOver(float delay)
        {
            Delay(GameOver, delay);
        }

        // Returns number of levels.
        public int GetLevels()
        {
            return levelSettings.Length;
        }

        // Is called when player selects the reset button.
        public void ResetButton()
        {
            if (screenSettings.reset == null) return;

            screenSettings.reset.gameObject.SetActive(true);

            Timing.Paused = true;
        }

        // Is called when player selects the yes button.
        public void ResetYes()
        {
            if (screenSettings.reset == null) return;

            screenSettings.reset.gameObject.SetActive(false);

            StartCoroutine(Load(ResetGame, screenSettings.inGame.gameObject, false, true, transition, advertisements));
        }

        // Is called when player selects the no button.
        public void ResetNo()
        {
            if (screenSettings.reset == null) return;

            screenSettings.reset.gameObject.SetActive(false);

            if (
                ((screenSettings.home && screenSettings.home.isActiveAndEnabled == false) || screenSettings.home == null) &&
                ((screenSettings.inGame && screenSettings.inGame.isActiveAndEnabled == true) || screenSettings.inGame == null)
                )
                Timing.Paused = false;
        }

        // Is called when player selects the settings button.
        public void SettingsButton()
        {
            if (screenSettings.settings == null) return;

            if (screenSettings.home && screenSettings.home.gameObject.activeInHierarchy) _back = screenSettings.home.gameObject;
            if (screenSettings.pause && screenSettings.pause.gameObject.activeInHierarchy) _back = screenSettings.pause.gameObject;
            if (screenSettings.gameOver && screenSettings.gameOver.gameObject.activeInHierarchy) _back = screenSettings.gameOver.gameObject;

            if (_back == null) return;

            _back.SetActive(false);

            _current = screenSettings.settings.gameObject;

            _current.SetActive(true);
        }

        // Is called when player selects the shop button.
        public void ShopButton()
        {
            if (screenSettings.settings == null) return;

            if (screenSettings.home && screenSettings.home.gameObject.activeInHierarchy) _back = screenSettings.home.gameObject;
            if (screenSettings.pause && screenSettings.pause.gameObject.activeInHierarchy) _back = screenSettings.pause.gameObject;
            if (screenSettings.gameOver && screenSettings.gameOver.gameObject.activeInHierarchy) _back = screenSettings.gameOver.gameObject;

            if (_back == null) return;

            _back.SetActive(false);

            _current = screenSettings.shop.gameObject;

            _current.SetActive(true);
        }

        // Retrurns the current level.
        public int GetCurrentLevel()
        {
            return PlayerPrefs.GetInt("levelIndex", 0) + 1;
        }

        // Will stop current level and load the next level.
        public void LevelUp()
        {
            StartCoroutine(Load(_GetNextLevel, screenSettings.inGame.gameObject, false, true, transition, advertisements));
        }

        // Will stop current level and reload it.
        public void Reload()
        {
            StartCoroutine(Load(SceneManager.GetActiveScene().name.ToString, screenSettings.inGame.gameObject, false, true, transition, advertisements));
        }

        public string LevelPick()
        {
            var index = PlayerPrefs.GetInt("levelIndex", 0);

            if (levelSettings.Length == 0 || index >= levelSettings.Length) return "";

            return levelSettings[index];
        }

        // Is called when player selects the pause button.
        public void PauseButton()
        {
            //if (isBusy || screenSettings.effects.messenger.isActiveAndEnabled == true) return;
            if (isBusy) return;

            if (screenSettings.pause == null || screenSettings.inGame == null) return;

            if (screenSettings.pause.gameObject.activeInHierarchy == true) return;

            screenSettings.inGame.gameObject.SetActive(false);

            screenSettings.pause.gameObject.SetActive(true);

            screenSettings.effects.SetPauseFader(1);

            //Timing.Paused = startPaused;
            Timing.Paused = true;
        }

        // Is called when player selects the play button.
        public void PlayButton()
        {
            if (screenSettings.home && screenSettings.home.gameObject.activeInHierarchy == true && screenSettings.inGame)
            {
                if (SceneManager.GetActiveScene().name == LevelPick())
                {
                    screenSettings.home.gameObject.SetActive(false);

                    screenSettings.inGame.gameObject.SetActive(true);

                    screenSettings.effects.SetPauseFader(-1);

                    Timing.Paused = false;
                }
                else
                {
                    StartCoroutine(Load(_StartGame, screenSettings.inGame.gameObject, false, true));
                }
            }
            else if (screenSettings.pause && screenSettings.pause.gameObject.activeInHierarchy == true && screenSettings.inGame && screenSettings.inGame.gameObject.activeInHierarchy == false)
            {
                screenSettings.pause.gameObject.SetActive(false);

                screenSettings.inGame.gameObject.SetActive(true);

                screenSettings.effects.SetPauseFader(-1);

                Timing.Paused = false;
            }
        }

        // Is called when player selects the replay button.
        public void ReplayButton()
        {
            StartCoroutine(Load(_StartGame, screenSettings.inGame.gameObject, false, true, transition, advertisements));
        }

        void Awake()
        {
#if UNITY_EDITOR
            if (ScenesInBuildDialog())
            {
                UnityEditor.EditorApplication.isPlaying = false;

                EditorWindow.GetWindow(System.Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));

                return;
            }
#endif
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;

            PlayerData.defaultLives = playerSettings.lives;

            if (screenSettings.home) screenSettings.home.gameObject.SetActive(false);
            if (screenSettings.inGame) screenSettings.inGame.gameObject.SetActive(false);
            if (screenSettings.about) screenSettings.about.gameObject.SetActive(false);
            if (screenSettings.gameOver) screenSettings.gameOver.gameObject.SetActive(false);
            if (screenSettings.pause) screenSettings.pause.gameObject.SetActive(false);
            if (screenSettings.settings) screenSettings.settings.gameObject.SetActive(false);
            if (screenSettings.shop) screenSettings.shop.gameObject.SetActive(false);
            if (screenSettings.loader) screenSettings.loader.gameObject.SetActive(false);
            if (screenSettings.reset) screenSettings.reset.gameObject.SetActive(false);

            if (screenSettings.home && screenSettings.home.enabled) screenSettings.hideObjectsByName.Hide(screenSettings.home.gameObject);
            if (screenSettings.inGame && screenSettings.inGame.enabled) screenSettings.hideObjectsByName.Hide(screenSettings.inGame.gameObject);
            if (screenSettings.about && screenSettings.about.enabled) screenSettings.hideObjectsByName.Hide(screenSettings.about.gameObject);
            if (screenSettings.gameOver && screenSettings.gameOver.enabled) screenSettings.hideObjectsByName.Hide(screenSettings.gameOver.gameObject);
            if (screenSettings.pause && screenSettings.pause.enabled) screenSettings.hideObjectsByName.Hide(screenSettings.pause.gameObject);
            if (screenSettings.settings && screenSettings.settings.enabled) screenSettings.hideObjectsByName.Hide(screenSettings.settings.gameObject);
            if (screenSettings.shop && screenSettings.shop.enabled) screenSettings.hideObjectsByName.Hide(screenSettings.shop.gameObject);
            if (screenSettings.loader && screenSettings.loader.enabled) screenSettings.hideObjectsByName.Hide(screenSettings.loader.gameObject);
            if (screenSettings.reset && screenSettings.reset.enabled) screenSettings.hideObjectsByName.Hide(screenSettings.reset.gameObject);

            screenSettings.effects.screenFaderImage.color = new Color(screenSettings.effects.screenFaderImage.color.r, screenSettings.effects.screenFaderImage.color.g, screenSettings.effects.screenFaderImage.color.b, 1);

            screenSettings.effects.gameObject.SetActive(true);

            musicSettings.Init(this);

            _Load();
        }

        string _GetBackgroundScene()
        {
            return backgroundScene;
        }

        string _GetLevel()
        {
            int levelIndex = PlayerPrefs.GetInt("levelIndex");
            if (levelIndex < 0 || levelIndex >= GetLevels()) return "null";

            return levelSettings[levelIndex];
        }

        string _GetNextLevel()
        {
            int levelIndex = PlayerPrefs.GetInt("levelIndex");
            if (levelIndex < 0 || levelIndex >= GetLevels()) levelIndex = 0;

            levelIndex += 1;
            if (levelIndex < 0 || levelIndex >= GetLevels()) levelIndex = 0;

            PlayerPrefs.SetInt("levelIndex", levelIndex);
            PlayerPrefs.Save();

            return levelSettings[levelIndex];
        }

        void _Load()
        {
            if (backgroundScene == "")
            {
                if ((screenSettings.pause && screenSettings.pauseAsMain) || (screenSettings.pause && screenSettings.home == null))
                {
                    StartCoroutine(Load(_StartGame, screenSettings.pause.gameObject, startPaused, true));
                }
                else if (screenSettings.home)
                {
                    StartCoroutine(Load(_StartGame, screenSettings.home.gameObject, startPaused, true));
                }
            }
            else if (screenSettings.home)
            {
                StartCoroutine(Load(_GetBackgroundScene, screenSettings.home.gameObject, startPaused, true));
            }
        }

        string _StartGame()
        {
            string scene = "";

            if (newGameEverytime)
            {
                scene = ResetGame();
            }
            else
            {
                PlayerData.Reset(playerSettings.lives); // Hack for now but check replay?

                var levelIndex = PlayerPrefs.GetInt("levelIndex");

                if (levelIndex >= GetLevels())
                {
                    PlayerPrefs.SetInt("levelIndex", levelIndex);
                    PlayerPrefs.Save();
                }

                scene = levelSettings[levelIndex];
            }

            return scene;
        }

#if UNITY_ANDROID
        void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus && !isBusy)
            {
                PauseButton();
            }

            //if (advertisementSettings.GetResult() != "Running") musicSettings.SetPause(!hasFocus);
            if (_intermission == false) musicSettings.SetPause(!hasFocus);
        }
#endif

#if UNITY_EDITOR || UNITY_IOS
        void OnApplicationPause(bool pause)
        {
            if (!pause && !isBusy)
            {
                PauseButton();
            }

            //if (advertisementSettings.GetResult() != "Running") musicSettings.SetPause(!pause);
            if (_intermission == false) musicSettings.SetPause(!pause);
        }
#endif
        void OnDestroy()
        {
            _instance = null;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
        }

        void Update()
        {
            if (_onTimerDone != null)
            {
                if (isMessengerBusy == false) _timer -= 1 * Time.deltaTime;

                if (_timer <= 0)
                {
                    _onTimerDone.Invoke();

                    _onTimerDone = null;
                }
            }
        }

#if UNITY_EDITOR
        public bool ScenesInBuildDialog()
        {
            var scenesInBuild = GetScenesInBuild();

            var missingScenes = GetAllMissingScenesInBuild();

            if (missingScenes.Count > 0)
            {
                var result = EditorUtility.DisplayDialog("Problem with Scenes In Build.", "This could be because the scenes used are not added to 'Scenes In Build' (Build Settings). \n\nDo you want the SimpleGameUI to try and fix it? \n\nIf you answer with yes the SimpleGameUI will try to add the scenes to the Unity 'Scenes In Build' and open the 'Build Settings' and display the results. \n\nYou must add the scenes manually if this doesn't solve your problem. \n\nThe simpleGameUI will quit in any case and you will need to run your program again.", "Yes, fix it", "No, will do this manually");

                if (result == true)
                {
                    scenesInBuild.AddRange(missingScenes);

                    EditorBuildSettings.scenes = scenesInBuild.ToArray();

                    return true;
                }
            }

            return false;
        }
        public List<EditorBuildSettingsScene> ScenesInBuild()
        {
            var scenesInBuild = GetScenesInBuild();

            var missingScenes = GetAllMissingScenesInBuild();

            scenesInBuild.AddRange(missingScenes);

            EditorBuildSettings.scenes = scenesInBuild.ToArray();

            return missingScenes;
        }

        public List<EditorBuildSettingsScene> GetScenesInBuild()
        {
            var scenesInBuild = new List<EditorBuildSettingsScene>();

            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                scenesInBuild.Add(scene);
            }

            return scenesInBuild;
        }

        public List<EditorBuildSettingsScene> GetAllMissingScenesInBuild()
        {
            string[] guids = AssetDatabase.FindAssets("t:Scene", searchInFolders);

            var allMissingScenes = GetMissingScenesInBuild(new string[] { Path.GetFileNameWithoutExtension(SceneManager.GetActiveScene().path) });

            allMissingScenes.AddRange(GetMissingScenesInBuild(new string[] { backgroundScene }));
            allMissingScenes.AddRange(GetMissingScenesInBuild(levelSettings));
            allMissingScenes.AddRange(GetMissingScenesInBuild(advertisements));
            allMissingScenes.AddRange(GetMissingScenesInBuild(new string[] { transition }));

            return allMissingScenes;

            List<EditorBuildSettingsScene> GetMissingScenesInBuild(string[] scenes)
            {
                var missingScenes = new List<EditorBuildSettingsScene>();

                foreach (string guid in guids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);

                    if (Wanted() && Contains() == false)
                    {
                        var scene = new EditorBuildSettingsScene(path, true);
                        missingScenes.Add(scene);
                    }

                    bool Wanted()
                    {
                        foreach (string name in scenes)
                        {
                            var file = Path.GetFileNameWithoutExtension(path);

                            if (name == file) return true;
                        }
                        return false;
                    }

                    bool Contains()
                    {
                        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
                        {
                            if (scene.path == path) return true;
                        }
                        return false;
                    }
                }

                return missingScenes;
            }
        }
        void LateUpdate()
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKeyDown(gameOver) && !isBusy) GameOver();
                if (Input.GetKeyDown(nextLevel) && !isBusy) LevelUp();
                if (Input.GetKeyDown(reloadLevel) && !isBusy) Reload();
                if (Input.GetKeyDown(resetGame)) ResetButton();
                //if (Input.GetKeyDown(KeyCode.P)) Timing.Paused = false;
            }
        }
#endif
        static SimpleGameUI _instance;

        bool _intermission;
        bool _loading;
        GameObject _back;
        GameObject _current;
        Action _onTimerDone;
        RectTransform _pauseRect;
        float _timer;
    }
}