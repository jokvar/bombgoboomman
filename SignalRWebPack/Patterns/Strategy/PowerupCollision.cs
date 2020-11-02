using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;

namespace SignalRWebPack.Patterns.Strategy
{
    class PowerupCollision : CollisionStrategy
    {

        public override void ExplosionCollisionStrategy(object collisionTarget, List<ExplosionCell> explosions, DateTime explodedAt, List<Powerup> collisionList)
        {
            var powerup = collisionTarget as Powerup;
            ExplosionCell exp1 = new ExplosionCell(explodedAt, powerup.x, powerup.y);
            explosions.Add(exp1);
            collisionList.RemoveAt(GetPowerupIndex(powerup, collisionList));
        }

        public override void PlayerCollisionStrategy(Player player, object collisionTarget, List<Powerup> collisionList)
        {
            var powerup = collisionTarget as Powerup;
            player.x = powerup.x;
            player.y = powerup.y;
            ResolvePowerup(player, powerup);
            collisionList.RemoveAt(GetPowerupIndex(powerup, collisionList));
        }

        private int GetPowerupIndex(Powerup powerup, List<Powerup> powerups)
        {
            for (int i = 0; i < powerups.Count; i++)
            {
                if (powerups[i] == powerup)
                {
                    return i;
                }
            }
            return 404; //not found
        }

        public void ResolvePowerup(Player playerReference, Powerup powerup)
        {
            switch (powerup.type)
            {
                //playerReference
                case Powerup_type.AdditionalBomb:
                    if (playerReference.maxBombs < 8)
                    {
                        playerReference.maxBombs++;
                    }
                    break;

                case Powerup_type.ExplosionSize:
                    playerReference.explosionSizeMultiplier++;
                    break;

                case Powerup_type.BombTickDuration:
                    if (playerReference.bombTickDuration > 2)
                    {
                        playerReference.bombTickDuration--;
                    }
                    break;

                //TODO: possibly reconsider use cases for these
                case Powerup_type.ExplosionDamage:
                    break;

                case Powerup_type.PlayerSpeed:
                    break;
            }
        }
    }
}
