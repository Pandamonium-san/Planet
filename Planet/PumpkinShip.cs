﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
    class PumpkinShip : Ship
    {
        //RadialWeapon<Projectile> wpn;
        Weapon<Projectile> wpn;

        public PumpkinShip(Vector2 pos)
            : base(pos)
        {
            SetTexture(AssetManager.GetTexture("pumpkin"));
            layer = Layer.ENEMY;

            wpn = new Weapon<Projectile>(this, 1, 20, 200, 4, 2.5f, 10.0f, 0.9f, 18, 90, 10, 0);
        }
        public override void Update(GameTime gt)
        {
            wpn.Update(gt);
            base.Update(gt);
        }
        protected override void CalculateCurrentRotation()
        {
            currentRotationSpeed = 1;
            rotation = MathHelper.WrapAngle(rotation);
        }
        public override void Fire1()
        {
            wpn.Fire();
        }

        public override void Fire2()
        {

        }

        public override void Fire3()
        {

        }

    }
}
