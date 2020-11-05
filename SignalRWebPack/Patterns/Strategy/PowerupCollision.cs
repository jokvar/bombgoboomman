using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;
using SignalRWebPack.Patterns.Command;

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
        public override void PlayerCollisionStrategy(Player player, object collisionTarget, List<Powerup> collisionList, PowerupInvoker powerupInvoker)
        {
            var powerup = collisionTarget as Powerup;
            player.x = powerup.x;
            player.y = powerup.y;
            ResolvePowerup(player, powerup, powerupInvoker);
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

        public void ResolvePowerup(Player playerReference, Powerup powerup, PowerupInvoker powerupInvoker)
        {
            PowerupCommand powerupCommand;
            switch (powerup.type)
            {
                case Powerup_type.BombTickDuration:
                    powerupCommand = new DecreaseBombTickDuration(playerReference);
                    break;
                case Powerup_type.ExplosionSize:
                    powerupCommand = new IncreaseExplosionSize(playerReference);
                    break;
                case Powerup_type.AdditionalBomb:
                    powerupCommand = new IncreaseBombCount(playerReference);
                    break;
                case Powerup_type.PowerDown:
                    powerupInvoker.Undo(1);
                    return;
                case Powerup_type.PowerDownX3:
                    powerupInvoker.Undo(3);
                    return;
                default:
                    powerupCommand = new IncreaseExplosionSize(playerReference);
                    break;
            }
            powerupInvoker.ExecuteCommand(powerupCommand);
        }
    }
}
