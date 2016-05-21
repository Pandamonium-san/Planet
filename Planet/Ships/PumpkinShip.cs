using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
    class PumpkinShip : Ship
    {
        Weapon wpn;

        public PumpkinShip(Vector2 pos)
            : base(pos)
        {
            SetTexture(AssetManager.GetTexture("pumpkin"));
            layer = Layer.ENEMY_SHIP;

            wpn = new CycloneGun(this);
        }
        protected override void DoUpdate(GameTime gt)
        {
            wpn.Update(gt);
            base.DoUpdate(gt);
        }
        protected override void CalculateCurrentRotation()
        {
            currentRotationSpeed = 1;
            Rotation = MathHelper.WrapAngle(Rotation);
        }
        public override void Fire1()
        {
           // wpn.Fire();
        }

        public override void Fire2()
        {

        }

        public override void Fire3()
        {

        }

    }
}
