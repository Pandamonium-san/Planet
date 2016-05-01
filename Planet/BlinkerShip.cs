using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
    class BlinkerShip : Ship
    {
        Weapon<Missile> wpn;
        public BlinkerShip(Vector2 pos)
            : base(pos)
        {
            layer = Layer.PLAYER;
            rotationSpeed = 15;
            wpn = new Weapon<Missile>(this, 1, 1, 500, 10, 30, 200);
        }
        public override void Update(GameTime gt)
        {
            wpn.Update(gt);
            base.Update(gt);
        }
        public override void Fire1()
        {
            wpn.Fire();
        }

        public override void Fire2()
        {
            Vector2 dir = currentVelocity;
            if (dir != Vector2.Zero)
                dir.Normalize();
            else
                dir = GetDirection();
            pos += dir * 100.0f;
        }

        public override void Fire3()
        {

        }


    }
}
