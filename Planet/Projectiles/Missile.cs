using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Planet
{
    class Missile : Projectile
    {
        public Missile(
       Vector2 pos,
       Vector2 dir,
       float speed,
       float damage = 1,
       GameObject instigator = null,
       float inaccuracy = 0,
       float lifeTime = 3)
            : base(AssetManager.GetTexture("Proj1"), pos, dir, speed, damage, instigator, lifeTime)
        {
            if (instigator == null)
            {
                layer = Layer.PLAYER_PROJECTILE | Layer.ENEMY_PROJECTILE;
                layerMask = ~Layer.ZERO;
            }
            else if (instigator.layer == Layer.PLAYER_SHIP)
            {
                layer = Layer.PLAYER_PROJECTILE;
                layerMask = (Layer.ENEMY_SHIP | Layer.ENEMY_PROJECTILE);
            }
            else if (instigator.layer == Layer.ENEMY_SHIP)
            {
                layer = Layer.ENEMY_PROJECTILE;
                layerMask = (Layer.PLAYER_SHIP | Layer.PLAYER_PROJECTILE);
            }
        }


    }
}
