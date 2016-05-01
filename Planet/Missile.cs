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
            : base(pos, dir, speed, damage, instigator, inaccuracy, lifeTime)
        {
        }
    }
}
