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

        protected float damage;
        protected float shotsPerSecond = 5;
        protected float shotDelay;
        protected float projectileSpeed;
        protected int nrOfBullets;
        protected float inaccuracy;
        protected float speedVariance;
        protected float volleyReloadTime;
        protected float currentReloadTime;
        protected int volleyAmount;
        protected int currentVolleyAmount;

        public Weapon(
            Ship ship,
            float damage,
            float shotsPerSecond,
            float projectileSpeed,
            int nrOfBullets = 1,
            float inaccuracy = 0,
            float speedVariance = 0,
            float volleyReloadTime = 0,
            int volleyAmount = 1)
        {
            this.ship = ship;
            this.damage = damage;
            this.shotsPerSecond = shotsPerSecond;
            this.projectileSpeed = projectileSpeed;
            this.nrOfBullets = nrOfBullets;
            this.inaccuracy = inaccuracy;
            this.speedVariance = speedVariance;
            this.volleyReloadTime = volleyReloadTime;
            this.volleyAmount = volleyAmount;
            this.currentVolleyAmount = volleyAmount;
        }
        
        public virtual void Update(GameTime gt)
        {
            shotDelay -= (float)gt.ElapsedGameTime.TotalSeconds;
            if(currentVolleyAmount < volleyAmount)
                currentReloadTime -= (float)gt.ElapsedGameTime.TotalSeconds;
        }

        public virtual void Fire()
        {
            if (currentVolleyAmount > 0)
            {
                if (shotDelay < 0)
                {
                    for (int i = 0; i < nrOfBullets; i++)
                    {
                        AddProjectile();
                    }
                    if (shotsPerSecond != 0)
                        shotDelay = 1 / shotsPerSecond;
                    currentVolleyAmount--;
                }
            }
            if (currentReloadTime <= 0)
            {
                currentReloadTime = volleyReloadTime;
                currentVolleyAmount = volleyAmount;
            }
        }

        protected virtual void AddProjectile()
        {
            float sv = (float)(Game1.rnd.NextDouble() * speedVariance - speedVariance / 2);
            object[] args = {
                ship.pos,
                ship.GetDirection(),
                sv + projectileSpeed, 
                damage, 
                ship, 
                inaccuracy, 
                3};
            Projectile projectile = (Projectile)Activator.CreateInstance(typeof(ProjType), args);

            //Projectile p = new Projectile(
            //    ship.pos,
            //    ship.GetDirection(),
            //    sv + projectileSpeed, 
            //    damage, 
            //    ship, 
            //    inaccuracy, 
            //    3);

            Game1.objMgr.PostGameObj(projectile);
        }

    }
}
