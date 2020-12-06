using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SignalRWebPack.Models;
using SignalRWebPack.Patterns.Command;
using Microsoft.Extensions.Logging;

namespace SignalRWebPack.Patterns.Strategy
{
    public class PowerupCollision : CollisionStrategy
    {
        public override void ExplosionCollisionStrategy(object collisionTarget, List<ExplosionCell> explosions, DateTime explodedAt, List<Powerup> collisionList)
        {
            if (collisionTarget == null || collisionTarget.GetType() != typeof(Powerup))
            {
                throw new InvalidOperationException("This method cannot be called when the type of collisionTarget is not 'Powerup'");
            }

            if(explosions == null)
            {
                throw new ArgumentNullException("This method cannot be called when the list 'explosions' is null");
            }

            var powerup = collisionTarget as Powerup;
            ExplosionCell exp1 = new ExplosionCell(explodedAt, powerup.x, powerup.y);
            explosions.Add(exp1);

            if (collisionList == null)
            {
                throw new ArgumentNullException("Powerup list cannot be null in this context");
            }

            collisionList.RemoveAt(GetPowerupIndex(powerup, collisionList));
        }
        public override void PlayerCollisionStrategy(Player player, object collisionTarget, List<Powerup> collisionList, PowerupInvoker powerupInvoker)
        {
            if (collisionTarget == null || collisionTarget.GetType() != typeof(Powerup))
            {
                throw new InvalidOperationException("This method cannot be called when the type of collisionTarget is not 'Powerup'");
            }

            if(player == null)
            {
                throw new ArgumentNullException("This method cannot be called when 'player' is null");
            }

            if (collisionList == null)
            {
                throw new ArgumentNullException("Powerup list cannot be null in this context");
            }

            var powerup = collisionTarget as Powerup;
            player.ExecuteAction();
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
            return -1; //not found
        }

        public void ResolvePowerup(Player playerReference, Powerup powerup, PowerupInvoker powerupInvoker)
        {
            if(powerup == null)
            {
                throw new ArgumentNullException("Cannot call this method when 'powerup' is null");
            }
            //0 - BombTickDuration - DecreaseBombTickDuration
            //1 - ExplosionSize - IncreaseExplosionSize
            //2 - AdditionalBomb - IncreaseBombCount
            //3 - PowerDown - Undo(1)
            //4 - PowerDownX3 - Undo(3)
            Console.WriteLine("Resolving powerup {0}", powerup.type);
            PowerupCommand powerupCommand = null;
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
                    break;
                case Powerup_type.PowerDownX3:
                    powerupInvoker.Undo(3);
                    break;
                default:
                    powerupCommand = new IncreaseExplosionSize(playerReference);
                    break;
            }
            if (powerupCommand == null)
            {
                return;
            }
            powerupInvoker.ExecuteCommand(powerupCommand);
        }
    }
}
