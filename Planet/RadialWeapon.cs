using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Planet
{
    class RadialWeapon<ProjType> : Weapon<ProjType> where ProjType : Projectile
    {
        protected float degreesBetweenBullets;
        protected float degreesBetweenShots;
        protected float currentBulletAngle;
        protected float currentShotAngle;

        public RadialWeapon(
            Ship ship,
            float damage,
            float shotsPerSecond,
            float projectileSpeed,
            float degreesBetweenBullets,
            float degreesBetweenVolleys,
            int nrOfBullets = 1,
            float inaccuracy = 0,
            float speedVariance = 0,
            float volleyReloadTime = 0,
            int volleyAmount = 1)
            : base(ship, damage, shotsPerSecond, projectileSpeed, nrOfBullets, inaccuracy, speedVariance, volleyReloadTime, volleyAmount)
        {
            this.degreesBetweenBullets = degreesBetweenBullets;
            this.degreesBetweenShots = degreesBetweenVolleys;
        }

        //only 3 lines more than base;
        public override void Fire()
        {
            if (currentReloadTime <= 0 && currentVolleyAmount > 0)
            {
                if (shotDelay < 0)
                {
                    currentBulletAngle = 0; //
                    for (int i = 0; i < nrOfBullets; i++)
                    {
                        AddProjectile();
                    }
                    currentShotAngle += MathHelper.ToRadians(degreesBetweenShots); //
                    if (shotsPerSecond != 0)
                        shotDelay = 1 / shotsPerSecond;
                    currentVolleyAmount--;
                }
            }
            else if (currentVolleyAmount <= 0)
            {
                currentReloadTime = volleyReloadTime;
                currentVolleyAmount = volleyAmount;
                currentShotAngle = 0; //
            }
        }

        //only 2 lines more than base
        protected override void AddProjectile()
        {
            Vector2 direction = Utility.AngleToVector2(ship.rotation + currentBulletAngle + currentShotAngle); //
            currentBulletAngle += MathHelper.ToRadians(degreesBetweenBullets); //
            float sv = (float)(Game1.rnd.NextDouble() * speedVariance - speedVariance / 2);
            object[] args = {
                ship.pos,
                direction, //
                sv + projectileSpeed, 
                damage, 
                ship, 
                inaccuracy, 
                3};
            Projectile projectile = (Projectile)Activator.CreateInstance(typeof(ProjType), args);

            Game1.objMgr.PostGameObj(projectile);
        }
    }
}
