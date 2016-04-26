using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
    class PumpkinShip : Ship
    {
        float reloadTime;
        float reloadDelay;
        int volleyAmount;
        int currentVolleyAmount;

        public PumpkinShip(Vector2 pos)
            : base(pos)
        {
            shotsPerSecond = 3;
            reloadTime = 5;
            reloadDelay = 0;
            volleyAmount = 4;
            currentVolleyAmount = volleyAmount;
        }
        public override void Update(GameTime gt)
        {
            reloadDelay -= (float)gt.ElapsedGameTime.TotalSeconds;
            base.Update(gt);
        }
        protected override void CalculateCurrentRotation()
        {
            currentRotationSpeed = 1;
            rotation = MathHelper.WrapAngle(rotation);
        }
        public override void Fire1()
        {
            if (reloadDelay < 0 && currentVolleyAmount > 0)
            {
                if (shotDelay < 0)
                {
                    Vector2 dir = Utility.AngleToVector2(rotation) * 5.0f;
                    Vector2 dir2 = new Vector2(dir.Y, -dir.X);
                    Vector2 dir3 = new Vector2(-dir.Y, dir.X);
                    Vector2 dir4 = -dir;

                    Vector2 pos1 = new Vector2(dir.Y, -dir.X) * 5;
                    Vector2 pos2 = new Vector2(dir.Y, -dir.X) * -5;

                    AddProjectile(new Projectile(pos, dir, 200, 10, this));
                    AddProjectile(new Projectile(pos, dir2, 200, 10, this));
                    AddProjectile(new Projectile(pos, dir3, 200, 10, this));
                    AddProjectile(new Projectile(pos, dir4, 200, 10, this));

                    shotDelay = 1 / shotsPerSecond;
                    --currentVolleyAmount;
                }
            }
            else if (currentVolleyAmount == 0)
            {
                reloadDelay = reloadTime;
                currentVolleyAmount = volleyAmount;
            }
        }

        public override void Fire2()
        {

        }

        public override void Fire3()
        {

        }

    }
}
