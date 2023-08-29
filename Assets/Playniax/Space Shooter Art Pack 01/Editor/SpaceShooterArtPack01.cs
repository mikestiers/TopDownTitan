using UnityEditor;
using UnityEngine;
using Playniax.Essentials.Editor;

namespace Playniax.SpaceShooterArtPack01.Editor
{
    public class Helpers : EditorWindowHelpers
    {
        [MenuItem("Playniax/Space Shooter Art Pack 01/Backgrounds/Basic/Scroller (Vertical)", false, 101)]
        public static void Add_Scroller_SP1_Basic_Vertical()
        {
            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Backgrounds/Basic/Scroller (Vertical).prefab");
        }

        [MenuItem("Playniax/Space Shooter Art Pack 01/Backgrounds/Basic/Scroller (Horizontal)", false, 101)]
        public static void Add_Scroller_SP1_Basic_Horizontal()
        {
            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Backgrounds/Basic/Scroller (Horizontal).prefab");
        }

        [MenuItem("Playniax/Space Shooter Art Pack 01/Backgrounds/Techno/Scroller (Horizontal)", false, 101)]
        public static void Add_Scroller_Techno_Horizontal()
        {
            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Backgrounds/Techno/Scroller (Horizontal).prefab");
        }

        [MenuItem("Playniax/Space Shooter Art Pack 01/Backgrounds/Techno/Scroller (Vertical)", false, 101)]
        public static void Add_Scroller_Techno_Vertical()
        {
            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Backgrounds/Techno/Scroller (Vertical).prefab");
        }

        [MenuItem("Playniax/Space Shooter Art Pack 01/Backgrounds/Crystals/Scroller (Horizontal)", false, 101)]
        public static void Add_Scroller_Crystals_Horizontal()
        {
            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Backgrounds/Crystals/Scroller (Horizontal).prefab");
        }

        [MenuItem("Playniax/Space Shooter Art Pack 01/Backgrounds/Crystals/Scroller (Vertical)", false, 101)]
        public static void Add_Scroller_Crystals_Vertical()
        {
            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Backgrounds/Crystals/Scroller (Vertical).prefab");
        }

        [MenuItem("Playniax/Space Shooter Art Pack 01/Backgrounds/Rocks/Scroller (Horizontal)", false, 101)]
        public static void Add_Scroller_Rocks_Horizontal()
        {
            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Backgrounds/Rocks/Scroller (Horizontal).prefab");
        }

        [MenuItem("Playniax/Space Shooter Art Pack 01/Backgrounds/Rocks/Scroller (Vertical)", false, 101)]
        public static void Add_Scroller_Rocks_Vertical()
        {
            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Backgrounds/Rocks/Scroller (Vertical).prefab");
        }

        [MenuItem("Playniax/Space Shooter Art Pack 01/Sprites/Players (Pickups)/3 Way Shooter", false, 101)]
        public static void Add_Pickup_3_Way_Shooter()
        {
            Prototyping.Editor.Helpers.Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Players (Pickups)/Pickup (3 Way Shooter).prefab");
        }
        [MenuItem("Playniax/Space Shooter Art Pack 01/Sprites/Players (Pickups)/Cannons", false, 101)]
        public static void Add_Pickup_Cannons()
        {
            Prototyping.Editor.Helpers.Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Players (Pickups)/Pickup (Cannons).prefab");
        }
        [MenuItem("Playniax/Space Shooter Art Pack 01/Sprites/Players (Pickups)/Health", false, 101)]
        public static void Add_Pickup_Health()
        {
            Prototyping.Editor.Helpers.Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Players (Pickups)/Pickup (Health).prefab");
        }
        [MenuItem("Playniax/Space Shooter Art Pack 01/Sprites/Players (Pickups)/Laser", false, 101)]
        public static void Add_Pickup_Laser()
        {
            Prototyping.Editor.Helpers.Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Players (Pickups)/Pickup (Laser).prefab");
        }
        [MenuItem("Playniax/Space Shooter Art Pack 01/Sprites/Players (Pickups)/Main Gun", false, 101)]
        public static void Add_Pickup_Main_Gun()
        {
            Prototyping.Editor.Helpers.Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Players (Pickups)/Pickup (Main Gun).prefab");
        }
        [MenuItem("Playniax/Space Shooter Art Pack 01/Sprites/Players (Pickups)/Phaser", false, 101)]
        public static void Add_Pickup_Phaser()
        {
            Prototyping.Editor.Helpers.Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Players (Pickups)/Pickup (Phaser).prefab");
        }
        [MenuItem("Playniax/Space Shooter Art Pack 01/Sprites/Players (Pickups)/Shield", false, 101)]
        public static void Add_Pickup_Shield()
        {
            Prototyping.Editor.Helpers.Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Players (Pickups)/Pickup (Shield).prefab");
        }
        [MenuItem("Playniax/Space Shooter Art Pack 01/Sprites/Players (Pickups)/Speed", false, 101)]
        public static void Add_Pickup_Speed()
        {
            Prototyping.Editor.Helpers.Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Players (Pickups)/Pickup (Speed).prefab");
        }
        [MenuItem("Playniax/Space Shooter Art Pack 01/Sprites/Players (Pickups)/Wrecking Ball", false, 101)]
        public static void Add_Pickup_Wrecking_Ball()
        {
            Prototyping.Editor.Helpers.Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Players (Pickups)/Pickup (Wrecking Ball).prefab");
        }

        [MenuItem("Playniax/Space Shooter Art Pack 01/Sprites/Players/Player (C64 Delta Style)", false, 101)]
        public static GameObject Add_Player()
        {
            Prototyping.Editor.Helpers.Add_Smart_Engine();

            return GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Players/Player (C64 Delta Style).prefab");
        }
        [MenuItem("Playniax/Space Shooter Art Pack 01/Sprites/Players/Player (C64 Delta Style Topview)", false, 101)]
        public static GameObject Add_Player_Topview()
        {
            Prototyping.Editor.Helpers.Add_Smart_Engine();

            return GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Players/Player (C64 Delta Style Topview).prefab");
        }

        [MenuItem("Playniax/Space Shooter Art Pack 01/Sprites/Enemies (Bosses)/Blade Boss", false, 101)]
        public static void Add_Blade_Boss()
        {
            Prototyping.Editor.Helpers.Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Enemies (Bosses)/Blade Boss.prefab");
        }

        [MenuItem("Playniax/Space Shooter Art Pack 01/Sprites/Enemies (Bosses)/Claw Boss", false, 101)]
        public static void Add_Spider_Boss()
        {
            Prototyping.Editor.Helpers.Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Enemies (Bosses)/Claw Boss.prefab");
        }

        [MenuItem("Playniax/Space Shooter Art Pack 01/Sprites/Enemies (Bosses)/Serpent Boss", false, 101)]
        public static void Add_Serpent_Boss()
        {
            Prototyping.Editor.Helpers.Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Enemies (Bosses)/Serpent Boss.prefab");
        }

        [MenuItem("Playniax/Space Shooter Art Pack 01/Sprites/Enemies/Atom", false, 101)]
        public static void Add_Atom()
        {
            Prototyping.Editor.Helpers.Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Enemies/Atom.prefab");
        }
        [MenuItem("Playniax/Space Shooter Art Pack 01/Sprites/Enemies/Blob", false, 101)]
        public static void Add_Blob()
        {
            Prototyping.Editor.Helpers.Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Enemies/Blob.prefab");
        }
        [MenuItem("Playniax/Space Shooter Art Pack 01/Sprites/Enemies/Cone", false, 101)]
        public static void Add_Cone()
        {
            Prototyping.Editor.Helpers.Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Enemies/Cone.prefab");
        }
        [MenuItem("Playniax/Space Shooter Art Pack 01/Sprites/Enemies/Gunner", false, 101)]
        public static void Add_Gunner()
        {
            Prototyping.Editor.Helpers.Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Enemies/Gunner.prefab");
        }
        [MenuItem("Playniax/Space Shooter Art Pack 01/Sprites/Enemies/Mohican", false, 101)]
        public static void Add_Mohican()
        {
            Prototyping.Editor.Helpers.Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Enemies/Mohican.prefab");
        }
        [MenuItem("Playniax/Space Shooter Art Pack 01/Sprites/Enemies/Pinchhead", false, 101)]
        public static void Add_Pinchhead()
        {
            Prototyping.Editor.Helpers.Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Enemies/Pinchhead.prefab");
        }
        [MenuItem("Playniax/Space Shooter Art Pack 01/Sprites/Enemies/Powered Rock", false, 101)]
        public static void Add_PoweredRock()
        {
            Prototyping.Editor.Helpers.Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Enemies/Powered Rock.prefab");
        }
        [MenuItem("Playniax/Space Shooter Art Pack 01/Sprites/Enemies/Ring", false, 101)]
        public static void Add_Ring()
        {
            Prototyping.Editor.Helpers.Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Enemies/Ring.prefab");
        }
        [MenuItem("Playniax/Space Shooter Art Pack 01/Sprites/Enemies/Satelite", false, 101)]
        public static void Add_Satelite()
        {
            Prototyping.Editor.Helpers.Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Enemies/Satelite.prefab");
        }
        [MenuItem("Playniax/Space Shooter Art Pack 01/Sprites/Enemies/Slicer", false, 101)]
        public static void Add_Slicer()
        {
            Prototyping.Editor.Helpers.Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Enemies/Slicer.prefab");
        }
        [MenuItem("Playniax/Space Shooter Art Pack 01/Sprites/Enemies/Sphere", false, 101)]
        public static void Add_Sphere()
        {
            Prototyping.Editor.Helpers.Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Enemies/Sphere.prefab");
        }
        [MenuItem("Playniax/Space Shooter Art Pack 01/Sprites/Enemies/Star", false, 101)]
        public static void Add_Star()
        {
            Prototyping.Editor.Helpers.Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Enemies/Star.prefab");
        }
        [MenuItem("Playniax/Space Shooter Art Pack 01/Sprites/Enemies/Starknife", false, 101)]
        public static void Add_Starknife()
        {
            Prototyping.Editor.Helpers.Add_Smart_Engine();

            GetAssetAtPath("Assets/Playniax/Space Shooter Art Pack 01/Prefabs/Enemies/Starknife.prefab");
        }
    }
}
