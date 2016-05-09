using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
    class BlinkerShip : Ship
    {
        Weapon<Projectile> wpn;
        Weapon<Projectile> wpn2;
        public BlinkerShip(Vector2 pos)
            : base(pos)
        {
            SetTexture(AssetManager.GetTexture("Parasite"));
            layer = Layer.PLAYER;
            rotationSpeed = 15;
            wpn = new RadialWeapon<Projectile>(this, 1, 5, 500, 5, 1, 5, 0, 0, 2, 20);
            wpn2 = new RadialWeapon<Projectile>(this, 1, 5, 500, -5, -1, 5, 0, 0, 2, 20);
        }
        public override void Update(GameTime gt)
        {
            wpn.Update(gt);
            wpn2.Update(gt);
            base.Update(gt);
        }
        public override void Fire1()
        {
            wpn.Fire();
            wpn2.Fire();
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
