using SignalRWebPack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack
{
    public class GameLogic
    {
        public List<Player> players = new List<Player>();

        public void RegisterPlayer(string playerName, string playerID)
        {
            Player player = new Player(playerName, playerID);
            players.Add(player);
        }

        public static void GameLoop()
        {
            while (true)
            {

            }
        }

        public static void UpdatePlayerPos(PlayerAction action)
        {
            switch (direction)
            {
                case "up":
                    break;
                case "down":
                    break;
                case "left":
                    break;
                case "right":
                    break;
            }
        }
    }
}
