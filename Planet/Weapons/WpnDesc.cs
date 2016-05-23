using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
    public class WpnDesc
    {
        public static WpnDesc Spread()
        {
            WpnDesc desc = new WpnDesc();
            desc.damage = 1;
            desc.shotsPerSecond = 5;
            desc.projectileSpeed = 500;
            desc.nrOfBullets = 5;
            desc.degreesBetweenBullets = 10;
            desc.startingAngleDegrees = -20;
            return desc;
        }
        public static WpnDesc Circle(int bullets)
        {
            WpnDesc desc = new WpnDesc();
            desc.damage = 1;
            desc.shotsPerSecond = 8;
            desc.projectileSpeed = 500;
            desc.nrOfBullets = bullets;
            desc.degreesBetweenBullets = 360.0f / bullets;

            return desc;
        }
        /// <summary>
        /// Contains information about the behaviour of a weapon.
        /// </summary>
        /// <param name="damage">Damage per projectile</param>
        /// <param name="shotsPerSecond">How fast the weapon can fire</param>
        /// <param name="projectileSpeed">How fast each projectile travels</param>
        /// <param name="nrOfBullets">Projectiles fired each shot</param>
        /// <param name="inaccuracy">In degrees, angle of each bullet changes by up to this amount</param>
        /// <param name="speedVariance">Projectile speed of each bullet changes by up to this amount</param>
        /// <param name="magReloadTime">Seconds to refill a non-full magazine to full</param>
        /// <param name="magSize">Amount of shots fired before reload is neccessary</param>
        /// <param name="degreesBetweenBullets">Degrees between bullets in a shot. Not relevant if nrOfBullets is 1</param>
        /// <param name="degreesBetweenShots">Degrees between shots in a magazine. Not relevant if magAmount is 1</param>
        /// <param name="startingAngleDegrees">First bullet in each shot is fired at an offset instead of going straight</param>
        /// <param name="projLifeTime">Time in seconds before bullet destroys itself</param>
        public WpnDesc(float damage,
            float shotsPerSecond,
            float projectileSpeed,
            int nrOfBullets,
            float inaccuracy,
            float speedVariance,
            float magReloadTime,
            int magSize,
            float degreesBetweenBullets,
            float degreesBetweenShots,
            float startingAngleDegrees ,
            float projLifeTime)
        {
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
            this.projLifeTime = projLifeTime;
        }

        public WpnDesc() { }

        public float damage = 1;
        public float shotsPerSecond = 1;
        public float projectileSpeed = 500;
        public int nrOfBullets = 1;
        public float inaccuracy = 0;
        public float speedVariance = 0;
        public float magReloadTime = 0;
        public int magSize = 1;
        public float degreesBetweenBullets = 0;
        public float degreesBetweenShots = 0;
        public float startingAngleDegrees = 0;
        public float projLifeTime = 5;
    }
}
