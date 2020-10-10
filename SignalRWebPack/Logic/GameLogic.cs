using Microsoft.AspNetCore.Http.Features;
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
        public Map gameMap = session.Map;
        public List<Bomb> bombs;
        public List<Explosion> explosions;
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

        public void GameLoop()
        {
            while (true)
            {
                Wall wall = new Wall();
                map.tiles[0] = wall;
            }
        }

        public void ProcessAction(PlayerAction playerAction, string id)
        {
            int requestIndex = session.MatchId(id);
            switch (playerAction.action)
            {
                case ActionEnums.Up:
                    players[requestIndex].y--;
                    break;
                case ActionEnums.Down:
                    players[requestIndex].y++;
                    break;
                case ActionEnums.Right:
                    players[requestIndex].x++;
                    break;
                case ActionEnums.Left:
                    players[requestIndex].x--;
                    break;
                case ActionEnums.PlaceBomb:
                    if(players[requestIndex].activeBombCount < players[requestIndex].maxBombs)
                    {
                        players[requestIndex].activeBombCount++;
                        PlaceBomb(requestIndex);
                    }
                    break;

            }
        }

        public void PlaceBomb(int requestIndex)
        {
            int x = players[requestIndex].x;
            int y = players[requestIndex].y;
            Bomb bomb = new Bomb(x, y, players[requestIndex]);
            bombs.Add(bomb);
        }

        //repeatedly called method for checking whether any bomb timers in the bomb list have expired yet
        public void CheckBombTimers()
        {
            for (int i = 0; i < bombs.Count; i++)
            {
                if(bombs[i].explodesAt <= DateTime.Now)
                {
                    BombExplosion(i);
                }
            }
        }

        //method for resolving bomb explosions
        public void BombExplosion(int bombIndex)
        {
            int x = bombs[bombIndex].x;
            int y = bombs[bombIndex].y;
            Player playerReference = bombs[bombIndex].placedBy;
            bombs.RemoveAt(bombIndex);
            SpawnExplosions(x, y, playerReference);
        }

        //spawns gunpowder with the center of the explosion being the coordinates x and y (where the bomb was initially placed)
        public void SpawnExplosions(int x, int y, Player playerReference)
        {
            DateTime explodedAt = DateTime.Now;
            Explosion explosion = new Explosion(explodedAt, x, y);
            DateTime expiresAt = explodedAt.AddSeconds(explosion.explosionDuration);
            //index = 15*y + x + 1
            int explosionCenterIndex = 15 * y + x + 1;

            //make 4 flags for each direction of the explosion to track whether it continues to spread or not
            bool xPlusStopped = false;
            bool xMinusStopped = false;
            bool yPlusStopped = false;
            bool yMinusStopped = false;

            //calculating explosion coordinates
            //replacing the initial bomb with an explosion tile
            explosions.Add(explosion);
            
            for (int i = 1; i <= explosion.size; i++)
            {
                //Explosion coordinate increase (x++)
                int explosionIndex = ConvertCoordsToIndex(x + i, y);

                //checking whether an explosion already exists at the given coordinates
                Explosion explosionCheck = explosions.Where(e => e.x == x + i && e.y == y).FirstOrDefault();

                if (gameMap.tiles[explosionIndex] is Wall)
                {
                    xPlusStopped = true;
                }
                //might not be null
                else if (explosionCheck != null)
                {
                    xPlusStopped = true;
                }
                else if (gameMap.tiles[explosionIndex] is Box && !xPlusStopped)
                {
                    Explosion exp1 = new Explosion(explodedAt, x + i, y);
                    explosions.Add(exp1);
                    xPlusStopped = true;
                }
                else if (gameMap.tiles[explosionIndex] is EmptyTile && !xPlusStopped)
                {
                    Explosion exp1 = new Explosion(explodedAt, x + i, y);
                    explosions.Add(exp1);
                }
                

                //Explosion coordinate increase (y++)
                explosionIndex = ConvertCoordsToIndex(x, y + i);

                //checking whether an explosion already exists at the given coordinates
                explosionCheck = explosions.Where(e => e.x == x && e.y == y + i).FirstOrDefault();

                if (gameMap.tiles[explosionIndex] is Wall)
                {
                    yPlusStopped = true;
                }
                else if (explosionCheck != null)
                {
                    yPlusStopped = true;
                }
                else if (gameMap.tiles[explosionIndex] is Box && !yPlusStopped)
                {
                    Explosion exp = new Explosion(explodedAt, x, y + i);
                    explosions.Add(exp);
                    yPlusStopped = true;
                }
                else if(gameMap.tiles[explosionIndex] is EmptyTile && !yPlusStopped)
                {
                    Explosion exp = new Explosion(explodedAt, x, y + i);
                    explosions.Add(exp);
                }


                //Explosion coordinate decrease (x--)
                explosionIndex = ConvertCoordsToIndex(x - i, y);

                //checking whether an explosion already exists at the given coordinates
                explosionCheck = explosions.Where(e => e.x == x - i && e.y == y).FirstOrDefault();

                if (gameMap.tiles[explosionIndex] is Wall)
                {
                    xMinusStopped = true;
                }
                else if (explosionCheck != null)
                {
                    xMinusStopped = true;
                }
                else if (gameMap.tiles[explosionIndex] is Box && !xMinusStopped)
                {
                    Explosion exp = new Explosion(explodedAt, x - i, y);
                    explosions.Add(exp);
                    xMinusStopped = true;
                }
                else if (gameMap.tiles[explosionIndex] is EmptyTile && !xMinusStopped)
                {
                    Explosion exp = new Explosion(explodedAt, x - i, y);
                    explosions.Add(exp);
                }

                //Explosion coordinate decrease (y--)
                explosionIndex = ConvertCoordsToIndex(x, y - i);

                //checking whether an explosion already exists at the given coordinates
                explosionCheck = explosions.Where(e => e.x == x && e.y == y - i).FirstOrDefault();

                if (gameMap.tiles[explosionIndex] is Wall)
                {
                    yMinusStopped = true;
                }
                else if (explosionCheck != null)
                {
                    yMinusStopped = true;
                }
                else if (gameMap.tiles[explosionIndex] is Box && !yMinusStopped)
                {
                    Explosion exp = new Explosion(explodedAt, x, y - i);
                    explosions.Add(exp);
                    xMinusStopped = true;
                }
                else if (gameMap.tiles[explosionIndex] is EmptyTile && !yMinusStopped)
                {
                    Explosion exp = new Explosion(explodedAt, x, y - i);
                    explosions.Add(exp);
                }
            }
        }

        public int ConvertCoordsToIndex(int x, int y)
        {
            return 15 * y + x + 1;
        }

        public void CheckInvulnerabilityPeriod()
        {

        }


        public void EnableDrawing(Session session)
        {

        }


    }
}
