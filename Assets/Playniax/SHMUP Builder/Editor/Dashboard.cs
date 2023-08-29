using UnityEditor;

using System.IO;
using UnityEngine;

using Playniax.Essentials.Editor;
using Playniax.Sequencer.Editor;

using Playniax.Ignition;
using Playniax.Sequencer;

using System;
using Playniax.Pyro;

namespace Playniax.Dashboard.Editor
{
    public class Dashboard : EditorWindowHelpers
    {
        public class Content : GUIContent
        {
            public string assetPath;

            public Content(string assetPath, string text, Texture2D texture, string tooltip)
            {
                this.assetPath = assetPath;
                this.text = text;
                image = texture;
                this.tooltip = tooltip;
            }
        }
        public class Record
        {
            public string assetPath = "";
            public string category = "";
            public string package = "";
            public Texture2D preview;
            public string previewPath = "";
            public string sections = "";
            public string spawners = "";
            public string title = "";
            public GUIContent content;

            public static Record[] Add(Record[] records, string category, string assetPath, string package, string sections, string spawners = "", string previewPath = "")
            {
                var folder = Path.GetDirectoryName(assetPath);

                if (AssetDatabase.IsValidFolder(folder) == false) return records;

                var title = Path.GetFileNameWithoutExtension(assetPath);

                var record = new Record();

                record.assetPath = assetPath;
                record.category = category;
                record.package = package;
                record.previewPath = previewPath;
                record.sections = sections;
                record.spawners = spawners;
                record.title = title;//.Replace(" ", "\n");

                records = ArrayHelpers.Add(records, record);

                return records;
            }

            public static Record[] Add(Record[] records, string category, string[] folders, string package, string sections, string spawners = "")
            {

                folders = FilterFolders(folders);

                if (folders.Length == 0) return records;

                var guids = AssetDatabase.FindAssets("t:GameObject", folders);

                for (int i = 0; i < guids.Length; i++)
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);

                    var title = Path.GetFileNameWithoutExtension(assetPath);

                    var record = new Record();

                    record.assetPath = assetPath;
                    record.category = category;
                    record.package = package;
                    record.sections = sections;
                    record.spawners = spawners;
                    record.title = title;//.Replace(" ", "\n");

                    records = ArrayHelpers.Add(records, record);
                }

                return records;
            }
        }

        public class Pickup
        {
            public string name;
            public string assetPath;

            public Pickup(string name, string assetPath)
            {
                this.name = name;
                this.assetPath = assetPath;
            }
        }

        public class Spawner
        {
            public string name;
            public Action onGUI;
            public Action<string> spawner;

            public Spawner(string name, Action onGUI, Action<string> spawner)
            {
                this.name = name;
                this.onGUI = onGUI;
                this.spawner = spawner;
            }
        }

        const string FOLDER = "Assets/Playniax/SHMUP Builder/Editor/Icons/";

        public static string[] FilterFolders(string[] folders)
        {
            var filterFolders = new string[0];

            for (int i = 0; i < folders.Length; i++)
            {
                if (AssetDatabase.IsValidFolder(folders[i])) filterFolders = ArrayHelpers.Add(filterFolders, folders[i]);
            }

            return filterFolders;
        }

        [MenuItem("Playniax/Dashboards/SHMUP Builder", false, 3)]
        static void Init()
        {
            var window = (Dashboard)GetWindow(typeof(Dashboard), false, "SHMUP Builder");

            window.minSize = new Vector2(266, 128);
        }

        void OnGUI()
        {
            GUI.skin.button = GetGUIStyle();

            //var toolbarOptions = new[] { GUILayout.MaxWidth(328) };

            //EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            _GUIPackage();

            var playersIcon = EditorGUIUtility.IconContent("d_UnityEditor.GameView").image;
            var libraryIcon = EditorGUIUtility.IconContent("d_Project").image;
            var sequencerIcon = EditorGUIUtility.IconContent("d_UnityEditor.SceneHierarchyWindow").image;

            _mainState = GUILayout.Toolbar(_mainState, new GUIContent[] {
                new GUIContent("Library", libraryIcon, "Library panel."),
                new GUIContent("Players", playersIcon, "Player panel."),
                new GUIContent("Sequencer", sequencerIcon, "Sequencer panel."),
                },
                GUILayout.Height(28));

            if (_mainState == 0)
            {
                _GUICategory();

                _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);

                var content = _GetContent("Library", category[_categoryState]);

                var buttons = Buttons(content, 72, 72) as Content;

                if (buttons != null)
                {
                    Prototyping.Editor.Helpers.Add_Smart_Engine();

                    GetAssetAtPath(buttons.assetPath);
                }

                GUILayout.EndScrollView();
            }
            else if (_mainState == 1)
            {
                _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);

                _GUIPlayerSettings();

                var content = _GetContent("Players");

                var buttons = Buttons(content, 72, 72);

                if (buttons != null)
                {
                    Prototyping.Editor.Helpers.Add_Smart_Engine();

                    var player = GetAssetAtPath(buttons.image.name);

                    _SetBulletSpawner(player, _triggerMode, _autofire, _rapidFire);
                    _SetBulletSpawners(player, _triggerMode, _autofire, _rapidFire);
                }

                GUILayout.EndScrollView();
            }
            else if (_mainState == 2)
            {
                var sequencerButtons = Buttons(new GUIContent[] {
                    new GUIContent("Message", AssetDatabase.LoadAssetAtPath<Texture2D>(FOLDER + "message.png"), "Adds the message command to the sequencer. Use this to display a message for the player."),
                    new GUIContent("Wait", AssetDatabase.LoadAssetAtPath<Texture2D>(FOLDER + "wait.png"), "Adds the wait command to the sequencer. Use this to have the sequencer wait for the current command to be finished."),
                    new GUIContent("Delay", AssetDatabase.LoadAssetAtPath<Texture2D>(FOLDER + "timer.png"), "Adds the delay command to the sequencer. Use this to create a pause between commands.")
                    }, 72, 72);

                _GetSpawners()[_sequencerIndex].onGUI();

                _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);

                if (sequencerButtons != null && sequencerButtons.text == "Message")
                {
                    Helpers.Add_Sequence_Message();
                }
                else if (sequencerButtons != null && sequencerButtons.text == "Wait")
                {
                    Helpers.Add_Sequence_WaitForSequenceToBeFinished();
                }
                else if (sequencerButtons != null && sequencerButtons.text == "Delay")
                {
                    Helpers.Add_Delay();
                }

                var content = _GetContent("Sequencer", "", _GetSpawners()[_sequencerIndex].name);

                var buttons = Buttons(content, 72, 72);

                if (buttons != null)
                    _GetSpawners()[_sequencerIndex].spawner(buttons.image.name);

                GUILayout.EndScrollView();
            }
        }

        void _AddInstantiator(string enemyPath)
        {
            var sequencer = _Get_Sequencer();
            if (sequencer == null) sequencer = Helpers.Add_Sequencer();

            var spawner = new GameObject(_spawners[_sequencerIndex].name).AddComponent<Instantiator>();
            spawner.transform.SetParent(sequencer.transform);

            spawner.prefab = (GameObject)AssetDatabase.LoadAssetAtPath(enemyPath, typeof(GameObject));

            Undo.RegisterCreatedObjectUndo(spawner.gameObject, "Create object");

            Selection.activeGameObject = spawner.gameObject;

        }
        void _AddSpawner(string enemyPath)
        {
            var fullName = @"Playniax.Sequencer." + _spawners[_sequencerIndex].name.Replace(" ", "") + ", Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
            var component = Type.GetType(fullName);

            var sequencer = _Get_Sequencer();
            if (sequencer == null) sequencer = Helpers.Add_Sequencer();

            var spawner = new GameObject(_spawners[_sequencerIndex].name).AddComponent(component) as AdvancedSpawnerBase;
            spawner.transform.SetParent(sequencer.transform);

            spawner.prefabs = new GameObject[] { (GameObject)AssetDatabase.LoadAssetAtPath(enemyPath, typeof(GameObject)) };

            spawner.SetBullet((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Playniax/Framework/Prototyping/Prefabs/Enemies (Bullets)/Enemy Bullet (Generic Red).prefab", typeof(GameObject)), _fireBullets);

            if (_coins) spawner.SetCoin((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Playniax/Framework/Prototyping/Prefabs/Players (Pickups)/Pickup (Coin).prefab", typeof(GameObject)));

            if (_pickups[_pickupIndex].name != "None")
            {
                spawner.SetPickup((GameObject)AssetDatabase.LoadAssetAtPath(_pickups[_pickupIndex].assetPath, typeof(GameObject)));
            }

            Undo.RegisterCreatedObjectUndo(spawner.gameObject, "Create object");

            Selection.activeGameObject = spawner.gameObject;
        }

        GUIContent[] _GetContent(string section, string category = "", string spawner = "")
        {
            _records = _GetLibrary(_records);

            var content = new GUIContent[0];

            for (int i = 0; i < _records.Length; i++)
            {
                Update();

                if (_records[i].preview != null) _records[i].preview.name = _records[i].assetPath;

                if (_records[i].package == package[_packageState] || package[_packageState] == "Show All")
                {
                    if (_records[i].sections.Contains(section) == false) continue;
                    if (spawner != "" && _records[i].spawners.Contains(spawner) == false) continue;
                    if (category != "" && category != "Show All" && _records[i].category != category) continue;

                    content = ArrayHelpers.Add(content, _records[i].content);
                }

                void Update()
                {
                    if (_records[i].preview == null)
                    {
                        if (_records[i].previewPath != "")
                        {
                            _records[i].preview = AssetDatabase.LoadAssetAtPath<Texture2D>(_records[i].previewPath);
                        }
                        else
                        {
                            _records[i].preview = GetPreview(_records[i].assetPath, 40, 40);
                        }

                        var display = _records[i].title; if (display.Length > 6) display = display[..6] + "...";
                        var preview = _records[i].preview;
                        var tooltip = _records[i].title;

                        tooltip += " belongs to the " + _records[i].category + " category in " + _records[i].package + " and the file path is " + _records[i].assetPath;

                        _records[i].content = new Content(_records[i].assetPath, display, preview, tooltip);
                    }
                }
            }

            return content;
        }

        Record[] _GetLibrary(Record[] records)
        {
            if (records == null)
            {
                // Prototyping

                records = Record.Add(records,
                    "Misc",
                    "Assets/Playniax/Framework/Prototyping/Prefabs/Engine/Pyro/Engine.prefab",
                    "Prototyping", "Library",
                    "",
                    "Assets/Playniax/Framework/Prototyping/Editor/Icons//thumb - pyro.png"); ;

                records = Record.Add(records, "Players", "Assets/Playniax/Framework/Prototyping/Prefabs/Players/Player (Spaceship Weaponized).prefab", "Prototyping", "Players", "");

                records = Record.Add(records, "Pickups", new[] { "Assets/Playniax/Framework/Prototyping/Prefabs/Players (Pickups)" }, "Prototyping", "Library");
                records = Record.Add(records, "Enemies", new[] { "Assets/Playniax/Framework/Prototyping/Prefabs/Enemies" }, "Prototyping", "Library,Sequencer", "Demo Spawner");

                records = Record.Add(records,
                    "UI",
                    "Assets/Playniax/Framework/Prototyping/Prefabs/UI/Player/Player Dash 1.prefab",
                    "Prototyping", "Library",
                    "",
                    "Assets/Playniax/Framework/Prototyping/Editor/Icons//thumb - player dash 1.png"); ;

                records = Record.Add(records,
                    "UI",
                    "Assets/Playniax/Framework/Prototyping/Prefabs/UI/Player/Player Dash 2.prefab",
                    "Prototyping", "Library",
                    "",
                    "Assets/Playniax/Framework/Prototyping/Editor/Icons//thumb - player dash 2.png"); ;

                records = Record.Add(records,
                    "Backgrounds",
                    "Assets/Playniax/Framework/Prototyping/Prefabs/Backgrounds/Scroller (Vertical).prefab",
                    "Prototyping", "Library",
                    "",
                    "Assets/Playniax/Framework/Prototyping/Editor/Icons/thumb - scroller.png");

                // Space Shooter Art Pack 01

                records = Record.Add(records, "Players", new[] { "Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Players" }, "Space Shooter Art Pack 01", "Players");

                records = Record.Add(records, "Enemies", new[] { "Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Enemies" }, "Space Shooter Art Pack 01", "Library,Sequencer", "Demo Spawner,Galaga Spawner,Invaders Spawner,Formation Spawner");

                records = Record.Add(records, "Enemies (Bosses)", "Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Enemies (Bosses)/Claw Boss.prefab", "Space Shooter Art Pack 01", "Library,Sequencer", "Formation Spawner");
                records = Record.Add(records, "Enemies (Bosses)", "Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Enemies (Bosses)/Blade Boss.prefab", "Space Shooter Art Pack 01", "Library,Sequencer", "Formation Spawner");
                records = Record.Add(records, "Enemies (Bosses)", "Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Enemies (Bosses)/Serpent Boss.prefab", "Space Shooter Art Pack 01", "Library,Sequencer", "Instantiator");

                records = Record.Add(records,
                    "Backgrounds",
                    "Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Backgrounds/Basic/Scroller (Horizontal).prefab",
                    "Space Shooter Art Pack 01", "Library",
                    "",
                    "Assets/Playniax/Space Shooter Art Pack 01/Editor/Icons//thumb - basic.png");

                records = Record.Add(records,
                    "Backgrounds",
                    "Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Backgrounds/Basic/Scroller (Vertical).prefab",
                    "Space Shooter Art Pack 01", "Library",
                    "",
                    "Assets/Playniax/Space Shooter Art Pack 01/Editor/Icons//thumb - basic.png");

                records = Record.Add(records,
                    "Backgrounds",
                    "Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Backgrounds/Crystals/Scroller (Horizontal).prefab",
                    "Space Shooter Art Pack 01", "Library",
                    "",
                    "Assets/Playniax/Space Shooter Art Pack 01/Editor/Icons//thumb - crystals.png");

                records = Record.Add(records,
                    "Backgrounds",
                    "Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Backgrounds/Crystals/Scroller (Vertical).prefab",
                    "Space Shooter Art Pack 01", "Library",
                    "",
                    "Assets/Playniax/Space Shooter Art Pack 01/Editor/Icons//thumb - crystals.png");

                records = Record.Add(records,
                    "Backgrounds",
                    "Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Backgrounds/Rocks/Scroller (Horizontal).prefab",
                    "Space Shooter Art Pack 01", "Library",
                    "",
                    "Assets/Playniax/Space Shooter Art Pack 01/Editor/Icons//thumb - rocks.png");

                records = Record.Add(records,
                    "Backgrounds",
                    "Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Backgrounds/Rocks/Scroller (Vertical).prefab",
                    "Space Shooter Art Pack 01", "Library",
                    "",
                    "Assets/Playniax/Space Shooter Art Pack 01/Editor/Icons//thumb - rocks.png");

                records = Record.Add(records,
                    "Backgrounds",
                    "Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Backgrounds/Techno/Scroller (Horizontal).prefab",
                    "Space Shooter Art Pack 01", "Library",
                    "",
                    "Assets/Playniax/Space Shooter Art Pack 01/Editor/Icons//thumb - techno.png");

                records = Record.Add(records,
                    "Backgrounds",
                    "Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Backgrounds/Techno/Scroller (Vertical).prefab",
                    "Space Shooter Art Pack 01", "Library",
                    "",
                    "Assets/Playniax/Space Shooter Art Pack 01/Editor/Icons//thumb - techno.png");

                records = Record.Add(records, "Pickups", new[] { "Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Players (Pickups)" }, "Space Shooter Art Pack 01", "Library");

                // Space Shooter Art Pack 02

                records = Record.Add(records, "Players", new[] { "Assets/Playniax/Space Shooter Art Pack 02/Prefabs/Players" }, "Space Shooter Art Pack 02", "Players");

                records = Record.Add(records, "Pickups", new[] { "Assets/Playniax/Space Shooter Art Pack 02/Prefabs/Players (Pickups)" }, "Space Shooter Art Pack 02", "Library");

                records = Record.Add(records, "Enemies", "Assets/Playniax/Space Shooter Art Pack 02/Prefabs/Enemies/Crusher.prefab", "Space Shooter Art Pack 02", "Library,Sequencer", "Instantiator");
                records = Record.Add(records, "Enemies", "Assets/Playniax/Space Shooter Art Pack 02/Prefabs/Enemies/Cube.prefab", "Space Shooter Art Pack 02", "Library,Sequencer", "Demo Spawner,Galaga Spawner,Invaders Spawner,Formation Spawner");
                records = Record.Add(records, "Enemies", "Assets/Playniax/Space Shooter Art Pack 02/Prefabs/Enemies/Launcher.prefab", "Space Shooter Art Pack 02", "Library");
                records = Record.Add(records, "Enemies", "Assets/Playniax/Space Shooter Art Pack 02/Prefabs/Enemies/Mine.prefab", "Space Shooter Art Pack 02", "Library,Sequencer", "Demo Spawner");
                records = Record.Add(records, "Enemies", "Assets/Playniax/Space Shooter Art Pack 02/Prefabs/Enemies/Mini Saucer.prefab", "Space Shooter Art Pack 02", "Library,Sequencer", "Demo Spawner,Galaga Spawner,Invaders Spawner,Formation Spawner");
                records = Record.Add(records, "Enemies", "Assets/Playniax/Space Shooter Art Pack 02/Prefabs/Enemies/Mushroom.prefab", "Space Shooter Art Pack 02", "Library,Sequencer", "Demo Spawner,Galaga Spawner,Invaders Spawner,Formation Spawner");
                records = Record.Add(records, "Enemies", "Assets/Playniax/Space Shooter Art Pack 02/Prefabs/Enemies/Pod Collector.prefab", "Space Shooter Art Pack 02", "Library,Sequencer", "Demo Spawner,Galaga Spawner,Invaders Spawner,Formation Spawner");
                records = Record.Add(records, "Enemies", "Assets/Playniax/Space Shooter Art Pack 02/Prefabs/Enemies/Pooper.prefab", "Space Shooter Art Pack 02", "Library,Sequencer", "Instantiator");
                records = Record.Add(records, "Enemies", "Assets/Playniax/Space Shooter Art Pack 02/Prefabs/Enemies/Pyramid.prefab", "Space Shooter Art Pack 02", "Library,Sequencer", "Demo Spawner,Galaga Spawner,Invaders Spawner,Formation Spawner");
                records = Record.Add(records, "Enemies", "Assets/Playniax/Space Shooter Art Pack 02/Prefabs/Enemies/Sphere Ring.prefab", "Space Shooter Art Pack 02", "Library");
                records = Record.Add(records, "Enemies", "Assets/Playniax/Space Shooter Art Pack 02/Prefabs/Enemies/Sphere.prefab", "Space Shooter Art Pack 02", "Library,Sequencer", "Demo Spawner,Galaga Spawner,Invaders Spawner,Formation Spawner");
                records = Record.Add(records, "Enemies", "Assets/Playniax/Space Shooter Art Pack 02/Prefabs/Enemies/Star.prefab", "Space Shooter Art Pack 02", "Library,Sequencer", "Demo Spawner,Galaga Spawner,Invaders Spawner,Formation Spawner");
                records = Record.Add(records, "Enemies", "Assets/Playniax/Space Shooter Art Pack 02/Prefabs/Enemies/Tank.prefab", "Space Shooter Art Pack 02", "Library");
                records = Record.Add(records, "Enemies", "Assets/Playniax/Space Shooter Art Pack 02/Prefabs/Enemies/Wobbler.prefab", "Space Shooter Art Pack 02", "Library,Sequencer", "Demo Spawner,Galaga Spawner,Invaders Spawner,Formation Spawner");

                records = Record.Add(records, "Enemies (Bosses)", "Assets/Playniax/Space Shooter Art Pack 02/Prefabs/Enemies (Bosses)/Big Boss.prefab", "Space Shooter Art Pack 02", "Library,Sequencer", "Demo Spawner,Formation Spawner");
                records = Record.Add(records, "Enemies (Bosses)", "Assets/Playniax/Space Shooter Art Pack 02/Prefabs/Enemies (Bosses)/Big Dish Boss.prefab", "Space Shooter Art Pack 02", "Library,Sequencer", "Demo Spawner,Formation Spawner");
                records = Record.Add(records, "Enemies (Bosses)", "Assets/Playniax/Space Shooter Art Pack 02/Prefabs/Enemies (Bosses)/Final Boss.prefab", "Space Shooter Art Pack 02", "Library,Sequencer", "Demo Spawner,Formation Spawner");
                records = Record.Add(records, "Enemies (Bosses)", "Assets/Playniax/Space Shooter Art Pack 02/Prefabs/Enemies (Bosses)/Mini Boss.prefab", "Space Shooter Art Pack 02", "Library,Sequencer", "Demo Spawner,Formation Spawner");
                records = Record.Add(records, "Enemies (Bosses)", "Assets/Playniax/Space Shooter Art Pack 02/Prefabs/Enemies (Bosses)/Satelite Boss.prefab", "Space Shooter Art Pack 02", "Library,Sequencer", "Formation Spawner");
                records = Record.Add(records, "Enemies (Bosses)", "Assets/Playniax/Space Shooter Art Pack 02/Prefabs/Enemies (Bosses)/Saucer Boss.prefab", "Space Shooter Art Pack 02", "Library,Sequencer", "Formation Spawner");
                records = Record.Add(records, "Enemies (Bosses)", "Assets/Playniax/Space Shooter Art Pack 02/Prefabs/Enemies (Bosses)/Spider Boss.prefab", "Space Shooter Art Pack 02", "Library,Sequencer", "Formation Spawner");

                records = Record.Add(records,
                    "Backgrounds",
                    "Assets/Playniax/Space Shooter Art Pack 02/Prefabs/Backgrounds/Basic/Scroller (Horizontal).prefab",
                    "Space Shooter Art Pack 02", "Library",
                    "",
                    "Assets/Playniax/Space Shooter Art Pack 02/Editor/Icons//thumb - basic.png"); ;

                records = Record.Add(records,
                    "Backgrounds",
                    "Assets/Playniax/Space Shooter Art Pack 02/Background Generator/Prefabs/Background Generator.prefab",
                    "Space Shooter Art Pack 02", "Library",
                    "",
                    "Assets/Playniax/Space Shooter Art Pack 02/Editor/Icons//thumb - background generator.png"); ;

                // Galaga Essentials

                records = Record.Add(records, "Players", new[] { "Assets/Playniax/Galaga Essentials/Prefabs/Players" }, "Galaga Essentials", "Players");

                records = Record.Add(records, "Enemies", new[] { "Assets/Playniax/Galaga Essentials/Prefabs/Enemies" }, "Galaga Essentials", "Library,Sequencer", "Demo Spawner,Galaga Spawner,Invaders Spawner,Formation Spawner");
//kut
                records = Record.Add(records,
                    "Backgrounds",
                    "Assets/Playniax/Galaga Essentials/Prefabs/Backgrounds/Galaga Scroller (Vertical).prefab",
                    "Galaga Essentials", "Library",
                    "",
                    "Assets/Playniax/Galaga Essentials/Editor/Icons/thumb - galaga scroller.png");

                records = Record.Add(records,
                    "Misc",
                    "Assets/Playniax/Galaga Essentials/Prefabs/Misc/Galaga Grid.prefab",
                    "Galaga Essentials", "Library",
                    "",
                    "Assets/Playniax/Galaga Essentials/Editor/Icons//thumb - galaga grid.png"); ;

                //for (int i = 0; i < records.Length; i++) Debug.Log(records[i].package + "|" + records[i].title + "|" + records[i].assetPath);
            }

            return records;
        }

        SequencerBase _Get_Sequencer()
        {
            SequencerBase sequencer;

            var all = FindObjectsOfType<SequencerBase>();
            if (all.Length == 0) return null;

            var active = Selection.activeGameObject;

            if (active)
            {
                sequencer = active.GetComponent<SequencerBase>();
                if (sequencer) return sequencer;
            }

            if (all[0]) return all[0];

            return null;
        }

        public string[] _GetPickupNames()
        {
            var names = new string[0];

            for (int i = 0; i < _pickups.Length; i++)
                names = ArrayHelpers.Add(names, _pickups[i].name);

            return names;
        }

        public string[] _GetSpawnerNames()
        {
            var names = new string[0];

            for (int i = 0; i < _GetSpawners().Length; i++)
                names = ArrayHelpers.Add(names, _spawners[i].name);

            return names;
        }

        void _GUIPackage()
        {
            if (package == null || package != null && package.Length == 0)
            {
                package = ArrayHelpers.Add(package, "Show All");

                if (AssetDatabase.IsValidFolder($"Assets/Playniax/Framework/Prototyping/Prefabs")) package = ArrayHelpers.Add(package, "Prototyping");
                if (AssetDatabase.IsValidFolder($"Assets/Playniax/Space Shooter Art Pack 01")) package = ArrayHelpers.Add(package, "Space Shooter Art Pack 01");
                if (AssetDatabase.IsValidFolder($"Assets/Playniax/Space Shooter Art Pack 02")) package = ArrayHelpers.Add(package, "Space Shooter Art Pack 02");
                if (AssetDatabase.IsValidFolder($"Assets/Playniax/Space Shooter Art Pack 03")) package = ArrayHelpers.Add(package, "Space Shooter Art Pack 03");
                if (AssetDatabase.IsValidFolder($"Assets/Playniax/Galaga Essentials")) package = ArrayHelpers.Add(package, "Galaga Essentials");
            }

            GUILayout.BeginHorizontal();

            GUILayout.Label("Package", GUILayout.ExpandWidth(false));

            _packageState = EditorGUILayout.Popup(_packageState, package, new[] { GUILayout.ExpandWidth(false) });

            GUILayout.EndHorizontal();
        }

        void _GUICategory()
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label("Category", GUILayout.ExpandWidth(false));

            _categoryState = EditorGUILayout.Popup(_categoryState, category, new[] { GUILayout.ExpandWidth(false) });

            GUILayout.EndHorizontal();
        }

        void _GUIPlayerSettings()
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label("Trigger Mode", GUILayout.ExpandWidth(false));

            _triggerMode = EditorGUILayout.Popup(_triggerMode, new string[] { "Auto", "Player", "Smart" }, new[] { GUILayout.ExpandWidth(false) });

            GUILayout.EndHorizontal();

            if (_triggerMode == 1)
            {
                GUILayout.Space(2);

                GUILayout.BeginHorizontal(); GUILayout.Space(TAB_SPACE); _autofire = GUILayout.Toggle(_autofire, "Autofire"); GUILayout.EndHorizontal();

                if (_autofire)
                {
                    GUILayout.BeginHorizontal(); GUILayout.Space(TAB_SPACE * 2); _rapidFire = GUILayout.Toggle(_rapidFire, "Rapid Fire"); GUILayout.EndHorizontal();
                }
            }
        }

        void _GUIGalagaSpawner()
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label("Sequencer", GUILayout.ExpandWidth(false));

            _sequencerIndex = EditorGUILayout.Popup(_sequencerIndex, _GetSpawnerNames(), new[] { GUILayout.ExpandWidth(false) });

            GUILayout.EndHorizontal();

            GUILayout.Space(4);

            GUILayout.BeginHorizontal();

            _fireBullets = GUILayout.Toggle(_fireBullets, "Fire Bullets");

            GUILayout.Space(TAB_SPACE);

            _coins = GUILayout.Toggle(_coins, "Add Coins");

            GUILayout.Label("Pickup", GUILayout.ExpandWidth(false));

            _pickupIndex = EditorGUILayout.Popup(_pickupIndex, _GetPickupNames(), new[] { GUILayout.ExpandWidth(false) });

            GUILayout.EndHorizontal();
        }

        void _GUIInstantiator()
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label("Sequencer", GUILayout.ExpandWidth(false));

            _sequencerIndex = EditorGUILayout.Popup(_sequencerIndex, _GetSpawnerNames(), new[] { GUILayout.ExpandWidth(false) });

            GUILayout.EndHorizontal();
        }

        Spawner[] _GetSpawners()
        {
            if (_spawners == null || _spawners != null && _spawners.Length == 0)
            {
                if (AssetDatabase.IsValidFolder($"Assets/Playniax/Galaga Essentials")) _spawners = ArrayHelpers.Add(_spawners, new Spawner("Galaga Spawner", _GUIGalagaSpawner, _AddSpawner));

                if (AssetDatabase.IsValidFolder($"Assets/Playniax/Framework/Advanced Spawners"))
                {
                    _spawners = ArrayHelpers.Add(_spawners, new Spawner("Demo Spawner", _GUIGalagaSpawner, _AddSpawner));
                    _spawners = ArrayHelpers.Add(_spawners, new Spawner("Invaders Spawner", _GUIGalagaSpawner, _AddSpawner));
                    _spawners = ArrayHelpers.Add(_spawners, new Spawner("Formation Spawner", _GUIGalagaSpawner, _AddSpawner));
                    _spawners = ArrayHelpers.Add(_spawners, new Spawner("Instantiator", _GUIInstantiator, _AddInstantiator));
                }
            }

            return _spawners;
        }

        void _SetBulletSpawner(GameObject gameObject, int triggerMode, bool autofire, bool rapidFire)
        {
            var spawners = gameObject.GetComponentsInChildren<BulletSpawner>();

            for (int i = 0; i < spawners.Length; i++)
            {
                if (spawners[i].mode == BulletSpawner.Mode.Direction)
                {
                    spawners[i].directionSettings.triggerSettings.mode = (BulletSpawner.DirectionSettings.TriggerSettings.Mode)triggerMode;
                    spawners[i].directionSettings.triggerSettings.autofire = autofire;
                    spawners[i].directionSettings.triggerSettings.rapidFire = rapidFire;
                }
            }
        }

        void _SetBulletSpawners(GameObject gameObject, int triggerMode, bool autofire, bool rapidFire)
        {
            var spawners = gameObject.GetComponentsInChildren<BulletSpawners>();

            for (int i = 0; i < spawners.Length; i++)
            {
                spawners[i].triggerSettings.mode = (BulletSpawners.TriggerSettings.Mode)triggerMode;
                spawners[i].triggerSettings.autofire = autofire;
                spawners[i].triggerSettings.rapidFire = rapidFire;
            }
        }

        Record[] _records;

        bool _coins;
        int _pickupIndex;
        int _sequencerIndex;
        bool _fireBullets;

        string[] category = new string[] { "Show All", "Pickups", "Enemies", "Enemies (Bosses)", "Backgrounds", "UI", "Misc" };

        int _categoryState;
        string[] package;
        int _packageState;
        int _mainState;
        Vector2 _scrollPosition;

        Pickup[] _pickups = new Pickup[] {
                new Pickup("None", ""),
                new Pickup("3 Way Shooter", "Assets/Playniax/Framework/Prototyping/Prefabs/Players (Pickups)/Pickup (3 Way Shooter).prefab"),
                new Pickup("Cannons", "Assets/Playniax/Framework/Prototyping/Prefabs/Players (Pickups)/Pickup (Cannons).prefab"),
                new Pickup("Drone", "Assets/Playniax/Framework/Prototyping/Prefabs/Players (Pickups)/Pickup (Drone).prefab"),
                new Pickup("Energy Beam", "Assets/Playniax/Framework/Prototyping/Prefabs/Players (Pickups)/Pickup (Energy Beam).prefab"),
                new Pickup("Extra Life", "Assets/Playniax/Framework/Prototyping/Prefabs/Players (Pickups)/Pickup (Extra Life).prefab"),
                new Pickup("Health", "Assets/Playniax/Framework/Prototyping/Prefabs/Players (Pickups)/Pickup (Health).prefab"),
                new Pickup("Laser", "Assets/Playniax/Framework/Prototyping/Prefabs/Players (Pickups)/Pickup (Laser).prefab"),
                new Pickup("Main Gun", "Assets/Playniax/Framework/Prototyping/Prefabs/Players (Pickups)/Pickup (Main Gun).prefab"),
                new Pickup("Missiles", "Assets/Playniax/Framework/Prototyping/Prefabs/Players (Pickups)/Pickup (Missiles).prefab"),
                new Pickup("Nuke", "Assets/Playniax/Framework/Prototyping/Prefabs/Players (Pickups)/Pickup (Nuke).prefab"),
                new Pickup("Phaser", "Assets/Playniax/Framework/Prototyping/Prefabs/Players (Pickups)/Pickup (Phaser).prefab"),
                new Pickup("Shield", "Assets/Playniax/Framework/Prototyping/Prefabs/Players (Pickups)/Pickup (Shield).prefab"),
                new Pickup("Wrecking Ball", "Assets/Playniax/Framework/Prototyping/Prefabs/Players (Pickups)/Pickup (Wrecking Ball).prefab")
            };

        Spawner[] _spawners;

        bool _autofire;
        bool _rapidFire;
        int _triggerMode;
    }
}
