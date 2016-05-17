using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
    //test class
    class CycloneGun : Weapon
    {

        public CycloneGun(Ship ship):base(ship, WpnDesc.Circle(24))
        {
        }

        public override void Update(GameTime gt)
        {

            base.Update(gt);
        }

        protected override void Shoot()
        {
            currentBulletAngle = MathHelper.ToRadians(startingAngleDegrees);
            Projectile center = new Projectile(null, ship.Pos, ship.GetDirection(), projSpeed, 0, null, 0, projLifeTime, null);
            center.Scale = 1.0f;
            center.layerMask = Layer.ZERO;
            Game1.objMgr.PostProjectile(center);
            for (int i = 0; i < nrOfBullets; i++)
            {
                CreateBullet(center);
                currentBulletAngle += MathHelper.ToRadians(degreesBetweenBullets);
            }
            currentShotAngle += MathHelper.ToRadians(degreesBetweenShots);
            if (shotsPerSecond != 0)
                secondsToNextShot = 1 / shotsPerSecond;
            currentMagCount--;
        }

        private void CreateBullet(Projectile center)
        {
            Vector2 direction = Utility.AngleToVector2(ship.Rotation + currentBulletAngle + currentShotAngle);
            if (inaccuracy != 0)
                ApplyInaccuracy(ref direction);
            float sv = Utility.GetRandom(Game1.rnd, -speedVariance, speedVariance);

            Projectile p = new Projectile(
                AssetManager.GetTexture("Proj1"),
                ship.Pos,
                direction,
                sv + projSpeed,
                damage,
                ship,
                inaccuracy,
                projLifeTime,
                BulletPattern);
            p.Parent = center;
            Game1.objMgr.PostProjectile(p);
            projectiles.Add(p);
        }
    }
}
