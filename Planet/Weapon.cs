using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Planet
{
    class Weapon<ProjType> where ProjType : Projectile
    {
        protected Ship ship;

        public float damage;
        public float shotsPerSecond;
        public int nrOfBullets;
        public float projectileSpeed;
        public float inaccuracy;
        public float speedVariance;

        //magazine variables
        public float magReloadTime;
        public int magSize;

        //bullet angle variables
        public float degreesBetweenBullets;
        public float degreesBetweenShots;
        public float startingAngleDegrees;

        //counter variables
        protected float secondsToNextShot;
        protected float secondsToNextReload;
        protected int currentMagCount;
        protected float currentBulletAngle;
        protected float currentShotAngle;

        public Weapon(
            Ship ship,
            float damage,
            float shotsPerSecond,
            float projectileSpeed,
            int nrOfBullets = 1,
            float inaccuracy = 0,
            float speedVariance = 0,
            float magReloadTime = 0,
            int magSize = 1,
            float degreesBetweenBullets = 0,
            float degreesBetweenShots = 0,
            float startingAngleDegrees = 0)
        {
            this.ship = ship;

            this.damage = damage;
            this.shotsPerSecond = shotsPerSecond;
            this.projectileSpeed = projectileSpeed;
            this.nrOfBullets = nrOfBullets;
            this.inaccuracy = inaccuracy;
            this.speedVariance = speedVariance;
            this.magReloadTime = magReloadTime;
            this.magSize = magSize;
            this.degreesBetweenBullets = degreesBetweenBullets;
            this.degreesBetweenShots = degreesBetweenShots;
            this.startingAngleDegrees = startingAngleDegrees;

            this.currentMagCount = magSize;
        }

        public Weapon(Ship ship, WpnDesc desc)
        {
            this.ship = ship;

            this.damage = desc.damage;
            this.shotsPerSecond = desc.shotsPerSecond;
            this.projectileSpeed = desc.projectileSpeed;
            this.nrOfBullets = desc.nrOfBullets;
            this.inaccuracy = desc.inaccuracy;
            this.speedVariance = desc.speedVariance;
            this.magReloadTime = desc.magReloadTime;
            this.magSize = desc.magSize;
            this.degreesBetweenBullets = desc.degreesBetweenBullets;
            this.degreesBetweenShots = desc.degreesBetweenShots;
            this.startingAngleDegrees = desc.startingAngleDegrees;

            this.currentMagCount = magSize;
        }

        public virtual void Update(GameTime gt)
        {
            secondsToNextShot -= (float)gt.ElapsedGameTime.TotalSeconds;

            //reload starts if volley is not full
            if (currentMagCount < magSize)
                secondsToNextReload -= (float)gt.ElapsedGameTime.TotalSeconds;
            if (secondsToNextReload <= 0)
                Reload();
        }

        public virtual void Fire()
        {
            if (currentMagCount > 0 && secondsToNextShot <= 0)
            {
                Shoot();
            }
        }

        private void Reload()
        {
            secondsToNextReload = magReloadTime;
            currentMagCount = magSize;
            currentShotAngle = 0;
        }

        private void Shoot()
        {
            currentBulletAngle = MathHelper.ToRadians(startingAngleDegrees);
            for (int i = 0; i < nrOfBullets; i++)
            {
                CreateBullet();
                currentBulletAngle += MathHelper.ToRadians(degreesBetweenBullets);
            }
            currentShotAngle += MathHelper.ToRadians(degreesBetweenShots);
            if (shotsPerSecond != 0)
                secondsToNextShot = 1 / shotsPerSecond;
            currentMagCount--;
        }

        protected virtual void CreateBullet()
        {
            Vector2 direction = Utility.AngleToVector2(ship.rotation + currentBulletAngle + currentShotAngle);
            if (inaccuracy != 0)
                ApplyInaccuracy(ref direction);
            float sv = Utility.GetRandom(Game1.rnd, -speedVariance, speedVariance);
            object[] args = {
                ship.pos,
                direction,
                sv + projectileSpeed, 
                damage, 
                ship, 
                inaccuracy, 
                3};
            Projectile projectile = (Projectile)Activator.CreateInstance(typeof(ProjType), args);

            Game1.objMgr.PostGameObj(projectile);
        }

        private void ApplyInaccuracy(ref Vector2 dir)
        {
            float deviation = Utility.GetRandom(Game1.rnd, -inaccuracy, inaccuracy);
            dir = Utility.RotateVector2(dir, MathHelper.ToRadians(deviation));
        }

        public WpnDesc GetDesc()
        {
            WpnDesc desc = new WpnDesc(
                this.damage,
                this.shotsPerSecond,
                this.projectileSpeed,
                this.nrOfBullets,
                this.inaccuracy,
                this.speedVariance,
                this.magReloadTime,
                this.magSize,
                this.degreesBetweenBullets,
                this.degreesBetweenShots,
                this.startingAngleDegrees);
            return desc;
        }
    }
}
