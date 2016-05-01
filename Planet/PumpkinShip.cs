using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
    class PumpkinShip : Ship
    {
        RadialWeapon<Projectile> wpn;

        public PumpkinShip(Vector2 pos)
            : base(pos)
        {
            SetTexture(AssetManager.GetTexture("pumpkin"));
            layer = Layer.ENEMY;

            wpn = new RadialWeapon<Projectile>(this, 1, 7, 350, 45, 10, 8, 0, 0, 5, 30);
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
            //if (reloadDelay < 0 && currentVolleyAmount > 0)
            //{
            //    if (shotDelay < 0)
            //    {
            //        Vector2 dir = Utility.AngleToVector2(rotation) * 5.0f;
            //        Vector2 dir2 = new Vector2(dir.Y, -dir.X);
            //        Vector2 dir3 = new Vector2(-dir.Y, dir.X);
            //        Vector2 dir4 = -dir;

            //        Vector2 pos1 = new Vector2(dir.Y, -dir.X) * 5;
            //        Vector2 pos2 = new Vector2(dir.Y, -dir.X) * -5;

            //        AddProjectile(new Projectile(pos, dir, 200, this));
            //        AddProjectile(new Projectile(pos, dir2, 200, this));
            //        AddProjectile(new Projectile(pos, dir3, 200, this));
            //        AddProjectile(new Projectile(pos, dir4, 200, this));

            //        shotDelay = 1 / shotsPerSecond;
            //        --currentVolleyAmount;
            //    }
            //}
            //else if (currentVolleyAmount == 0)
            //{
            //    reloadDelay = reloadTime;
            //    currentVolleyAmount = volleyAmount;
            //}
        }

        public override void Fire2()
        {

        }

        public override void Fire3()
        {

        }

    }
}
