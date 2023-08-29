using UnityEditor;
using UnityEngine;
using Playniax.Ignition;
using Playniax.Pyro;
using Playniax.ParticleSystem;
using Playniax.Essentials.Editor;

namespace Playniax.Prototyping.Editor
{
    public class Helpers : EditorWindowHelpers
    {
        public static void Add_Smart_Engine()
        {
            var audioChannels = Object.FindObjectOfType<AudioChannels>();
            var collisionAudio = Object.FindObjectOfType<CollisionAudio>();
            var collisionMonitor = Object.FindObjectOfType<CollisionMonitor2D>();
            var messenger = Object.FindObjectOfType<Messenger>();
            var emitter = Object.FindObjectOfType<EmitterGroup>();

            if (audioChannels) return;
            if (collisionAudio) return;
            if (collisionMonitor) return;
            if (messenger) return;
            if (emitter) return;

            Add_Engine();

            Selection.activeGameObject = null;
        }


        [MenuItem("Playniax/Prototyping/UI/Player Dash 1", false, 61)]
        public static void Add_PlayerDash1()
        {
            GetAssetAtPath("Assets/Playniax/Framework/Prototyping/Prefabs/UI/Player/Player Dash 1.prefab");
        }

        [MenuItem("Playniax/Prototyping/UI/Player Dash 2", false, 61)]
        public static void Add_PlayerDash2()
        {
            GetAssetAtPath("Assets/Playniax/Framework/Prototyping/Prefabs/UI/Player/Player Dash 2.prefab");
        }

        [MenuItem("Playniax/Prototyping/Engines/Pyro", false, 61)]
        public static void Add_Engine()
        {
            var engine = new GameObject("Engine");

            Undo.RegisterCreatedObjectUndo(engine.gameObject, "Create object");

            Selection.activeGameObject = engine.gameObject;

            GetAssetAtPath("Assets/Playniax/Framework/Prototyping/Prefabs/Engine/Pyro/Audio Channels.prefab", true);
            Selection.activeGameObject = engine.gameObject;
            GetAssetAtPath("Assets/Playniax/Framework/Prototyping/Prefabs/Engine/Pyro/Collision Audio.prefab", true);
            Selection.activeGameObject = engine.gameObject;
            GetAssetAtPath("Assets/Playniax/Framework/Prototyping/Prefabs/Engine/Pyro/Collision Monitor.prefab", true);
            Selection.activeGameObject = engine.gameObject;
            GetAssetAtPath("Assets/Playniax/Framework/Prototyping/Prefabs/Engine/Pyro/Messenger.prefab", true);
            Selection.activeGameObject = engine.gameObject;
            GetAssetAtPath("Assets/Playniax/Framework/Prototyping/Prefabs/Engine/Pyro/Particle Effects.prefab", true);
            Selection.activeGameObject = engine.gameObject;
        }

        [MenuItem("Playniax/Prototyping/Sprites/Players/Player (Spaceship)", false, 61)]
        public static void Add_Player()
        {
            Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Framework/Prototyping/Prefabs/Players/Player (Spaceship).prefab");
        }

        [MenuItem("Playniax/Prototyping/Sprites/Players/Player (Spaceship Weaponized)", false, 61)]
        public static GameObject Add_Player_Weaponized()
        {
            Add_Smart_Engine();

            return GetAssetAtPath("Assets/Playniax/Framework/Prototyping/Prefabs/Players/Player (Spaceship Weaponized).prefab");
        }

        [MenuItem("Playniax/Prototyping/Sprites/Enemies/Enemy", false, 61)]
        public static void Enemey_01()
        {
            Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Framework/Prototyping/Prefabs/Enemies/Enemy.prefab");
        }

        [MenuItem("Playniax/Prototyping/Sprites/Enemies/Rocket", false, 61)]
        public static void Add_Rocket()
        {
            Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Framework/Prototyping/Prefabs/Enemies/Rocket.prefab");
        }

        [MenuItem("Playniax/Prototyping/Sprites/Pickups/Pickup (3 Way Shooter)", false, 61)]
        public static void Add_Pickup_3_Way_Shooter()
        {
            Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Framework/Prototyping/Prefabs/Players (Pickups)/Pickup (3 Way Shooter).prefab");
        }

        [MenuItem("Playniax/Prototyping/Sprites/Pickups/Pickup (Cannons)", false, 61)]
        public static void Add_Pickup_Cannons()
        {
            Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Framework/Prototyping/Prefabs/Players (Pickups)/Pickup (Cannons).prefab");
        }

        [MenuItem("Playniax/Prototyping/Sprites/Pickups/Pickup (Coin)", false, 61)]
        public static void Add_Pickup_Coin()
        {
            Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Framework/Prototyping/Prefabs/Players (Pickups)/Pickup (Coin).prefab");
        }

        [MenuItem("Playniax/Prototyping/Sprites/Pickups/Pickup (Drone)", false, 61)]
        public static void Add_Pickup_Drone()
        {
            Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Framework/Prototyping/Prefabs/Players (Pickups)/Pickup (Drone).prefab");
        }

        [MenuItem("Playniax/Prototyping/Sprites/Pickups/Pickup (Energy Beam)", false, 61)]
        public static void Add_Pickup_Energy_Beam()
        {
            Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Framework/Prototyping/Prefabs/Players (Pickups)/Pickup (Energy Beam).prefab");
        }

        [MenuItem("Playniax/Prototyping/Sprites/Pickups/Pickup (Extra Life)", false, 61)]
        public static void Add_Pickup_Extra_Life()
        {
            Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Framework/Prototyping/Prefabs/Players (Pickups)/Pickup (Extra Life).prefab");
        }

        [MenuItem("Playniax/Prototyping/Sprites/Pickups/Pickup (Health)", false, 61)]
        public static void Add_Pickup_Health()
        {
            Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Framework/Prototyping/Prefabs/Players (Pickups)/Pickup (Health).prefab");
        }

        [MenuItem("Playniax/Prototyping/Sprites/Pickups/Pickup (Laser)", false, 61)]
        public static void Add_Pickup_Laser()
        {
            Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Framework/Prototyping/Prefabs/Players (Pickups)/Pickup (Laser).prefab");
        }

        [MenuItem("Playniax/Prototyping/Sprites/Pickups/Pickup (Main Gun)", false, 61)]
        public static void Add_Pickup_Main_Gun()
        {
            Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Framework/Prototyping/Prefabs/Players (Pickups)/Pickup (Main Gun).prefab");
        }

        [MenuItem("Playniax/Prototyping/Sprites/Pickups/Pickup (Missiles)", false, 61)]
        public static void Add_Pickup_Missiles()
        {
            Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Framework/Prototyping/Prefabs/Players (Pickups)/Pickup (Missiles).prefab");
        }

        [MenuItem("Playniax/Prototyping/Sprites/Pickups/Pickup (Nuke)", false, 61)]
        public static void Add_Pickup_Nuke()
        {
            Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Framework/Prototyping/Prefabs/Players (Pickups)/Pickup (Nuke).prefab");
        }

        [MenuItem("Playniax/Prototyping/Sprites/Pickups/Pickup (Phaser)", false, 61)]
        public static void Add_Pickup_Phaser()
        {
            Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Framework/Prototyping/Prefabs/Players (Pickups)/Pickup (Phaser).prefab");
        }

        [MenuItem("Playniax/Prototyping/Sprites/Pickups/Pickup (Shield)", false, 61)]
        public static void Add_Pickup_Shield()
        {
            Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Framework/Prototyping/Prefabs/Players (Pickups)/Pickup (Shield).prefab");
        }

        [MenuItem("Playniax/Prototyping/Sprites/Pickups/Pickup (Wrecking Ball)", false, 61)]
        public static void Add_Pickup_Wrecking_Ball()
        {
            Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Framework/Prototyping/Prefabs/Players (Pickups)/Pickup (Wrecking Ball).prefab");
        }
    }
}


