namespace Playniax.Ignition
{
    [System.Serializable]
    // PlayerData can be used to temporarily store data of the game like lives, name or score of the player. 
    //
    // lives, name of score are built-in data fields.
    //
    // Custom fields can be added to custom and supports floats, integers, booleans and strings.
    //
    // This information will not be saved and is only avaiable at runtime.
    //
    // Example(s):
    //
    // Set name of the player              : PlayerData.Get(0).name = "Tony"
    //
    // Increase the score of the player    : PlayerData.Get(0).scoreboard += 100;
    //
    // Custom variable                     : PlayerData.Get(0).custom.SetBool("invincible", true);
    public class PlayerData
    {
        public static int defaultLives = 3;

        public static PlayerData[] data;

        public int coins;
        public int lives = defaultLives;
        public string name = "";
        public int scoreboard;

        //public List<string> rewards;

        // Custom data storage.
        public Config custom = new Config();
        public static int CountLives()
        {
            if (data == null) return 0;

            var lives = 0;

            for (int i = 0; i < data.Length; i++)
            {
                lives += data[i].lives;
            }
            
            return lives;
        }
        // Returns the PlayerData for the player by index.
        public static PlayerData Get(int index = 0)
        {
            if (index < 0) return null;
            if (data == null) System.Array.Resize(ref data, index + 1);
            if (index >= data.Length) System.Array.Resize(ref data, index + 1);
            if (data[index] == null) data[index] = new PlayerData();

            return data[index];
        }
        // Returns the PlayerData for the player by name.
        public static PlayerData Get(string name)
        {
            if (data != null)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i].name == name) return Get(i);
                }

            }

            return null;
        }
        // Reset to defaults
        public static void Reset(int lives)
        {
            if (data == null) return;

            for (int i = 0; i < data.Length; i++)
            {
                data[i].coins = 0;
                data[i].lives = lives;
                data[i].scoreboard = 0;

                //if (data[i].rewards != null) data[i].rewards.Clear();

                data[i].custom.Clear();
            }
        }
        // Set number of lives
        public static void SetLives(int lives)
        {
            if (data == null) return;

            for (int i = 0; i < data.Length; i++)
            {
                data[i].lives = lives;
            }
        }
    }
}