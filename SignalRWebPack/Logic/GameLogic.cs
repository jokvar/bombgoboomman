using SignalRWebPack.Logic;
using SignalRWebPack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack
{
    public class GameLogic
    {
        public static Session session = SessionManager.GetSession();
        public List<Player> players = session.Players;
        public Map map = session.Map;
        public List<Bomb> bombs;
        private int playerIndex = 0;
        public int mapDimensions = 15;

        public void SpawnPlayers()
        {
            players[0].x = 1;
            players[0].y = 1;

            players[1].x = mapDimensions - 1;
            players[1].y = 1;

            players[2].x = 1;
            players[2].y = mapDimensions - 1;

            players[3].x = mapDimensions - 1;
            players[3].y = mapDimensions - 1;
        }

        public static void GameLoop()
        {
            while (true)
            {

            }
        }

        public void UpdatePlayerPos(PlayerAction action, string id)
        {
            int requestIndex = session.MatchId(id);
            switch (action.direction)
            {
                case "up":
                    players[requestIndex].y++;
                    break;
                case "down":
                    players[requestIndex].y--;
                    break;
                case "right":
                    players[requestIndex].x++;
                    break;
                case "left":
                    players[requestIndex].x--;
                    break;
            }
        }

        public void PlaceBomb(string id)
        {
            int requestIndex = session.MatchId(id);
            int x = players[requestIndex].x;
            int y = players[requestIndex].y;
            Bomb bomb = new Bomb(x, y);
            bombs.Add(bomb);
        }

        public void CheckBombTimers()
        {
            for (int i = 0; i < bombs.Count; i++)
            {
                if(bombs[i].explodesAt <= DateTime.Now)
                {
                    ExplodeOrSomething();
                }
            }
        }



        public void EnableDrawing(Session session)
        {

        }


    }
}
