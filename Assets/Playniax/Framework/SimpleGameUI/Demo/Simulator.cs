using UnityEngine;
using Playniax.Ignition;

namespace Playniax.UI.SimpleGameUI
{
    /*

        To see if the game is paused use:

        Timing.Paused

        ( returns True or False )

    */

    public class Simulator : MonoBehaviour
    {
        public int playerIndex;

        private void Awake()
        {
            if (SimpleGameUI.instance && SimpleGameUI.instance.isLastLevel) print("Player just entered last level");
        }

        public void LevelUp()
        {
            if (SimpleGameUI.instance) SimpleGameUI.instance.LevelUp();        // Call this when the player successfully finishes a level
        }

        public void LifeLoss()
        {
            PlayerData.Get(playerIndex).lives -= 1;     // Use the built-in counter to keep track of how many lifes the player has left

            if (PlayerData.Get(playerIndex).lives <= 0) GameOver();     // No lifes left? Game Over
        }

        public void GameOver()
        {
            if (SimpleGameUI.instance) SimpleGameUI.instance.GameOver();       // Call this when the game is over
        }

        public void Score()
        {
            PlayerData.Get(playerIndex).scoreboard += 10;     // Use the built-in counter to keep track of how much points the player scores
        }
    }
}