﻿using SignalRWebPack.Logic;
using SignalRWebPack.Models;
using SignalRWebPack.Patterns.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Patterns.Strategy
{
    class BombCollision : CollisionStrategy
    {
        public override void ExplosionCollisionStrategy(object collisionTarget, List<ExplosionCell> explosions, DateTime explodedAt, List<Powerup> powerupList)
        {
            var bomb = collisionTarget as Bomb;
            bomb.Explode();
        }

        public override void PlayerCollisionStrategy(Player player, object collisionTarget, List<Powerup> collisionList, PowerupInvoker powerupInvoker)
        {
            var bomb = collisionTarget as Bomb;
            if (bomb.hasExploded)
            {
                player.x = bomb.x;
                player.y = bomb.y;
                if (!player.invulnerable)
                {
                    player.lives--;
                    Session s = SessionManager.Instance.GetPlayerSession(player.id);
                    s.LastPlayerDamaged = player;
                    s.Notify();
                    player.invulnerableSince = DateTime.Now;
                    player.invulnerableUntil = player.invulnerableSince.AddSeconds(player.invulnerabilityDuration);
                    player.invulnerable = true;
                }
            }
        }
    }
}
