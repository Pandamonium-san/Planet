using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
    class BlinkerShip : Ship
    {
        Weapon wpn;
        public BlinkerShip(Vector2 pos)
            : base(pos)
        {
            hitboxOffset = new Vector2(10, 10);
            SetTexture(AssetManager.GetTexture("Parasite"));
            layer = Layer.PLAYER_SHIP;
            rotationSpeed = 15;
            //wpn = new Weapon(this, WpnDesc.Spread());
            wpn = new Weapon(this, WpnDesc.Circle(24));
            wpn = new CycloneGun(this);
            wpn.projSpeed = 200;
            wpn.projLifeTime = 3;
            wpn.magSize = 5;
            wpn.magReloadTime = 3;
            //wpn.inaccuracy = 1;
            //wpn.speedVariance = 50;
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
            Pos += dir * 100.0f;
        }

        public override void Fire3()
        {

        }


    }
}
