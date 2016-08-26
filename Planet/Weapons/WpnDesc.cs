using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  public struct WpnDesc
  {
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
    /// <param name="startingAngleDegrees">First bullet in each shot is fired at an angle instead of going straight</param>
    /// <param name="projLifeTime">Time in seconds before bullet destroys itself</param>
    /// <param name="ignoreRotation">If true, does not take into account the ship's rotation when determining shot direction</param>
    public WpnDesc(
        int damage,
        float shotsPerSecond,
        float projectileSpeed,
        int nrOfBullets = 1,
        float inaccuracy = 0,
        float speedVariance = 0,
        float magReloadTime = 0,
        int magSize = 1,
        float degreesBetweenBullets = 0,
        float degreesBetweenShots = 0,
        float startingAngleDegrees = 0,
        float projLifeTime = 10,
        bool ignoreRotation = false)
    {
      this.damage = damage;
      this.shotsPerSecond = shotsPerSecond;
      this.projSpeed = projectileSpeed;
      this.nrOfBullets = nrOfBullets;
      this.inaccuracy = inaccuracy;
      this.speedVariance = speedVariance;
      this.magReloadTime = magReloadTime;
      this.magSize = magSize;
      this.degreesBetweenBullets = degreesBetweenBullets;
      this.degreesBetweenShots = degreesBetweenShots;
      this.startingAngleDegrees = startingAngleDegrees;
      this.projLifeTime = projLifeTime;
      this.ignoreRotation = ignoreRotation;
    }

    public int damage;
    public float shotsPerSecond;
    public float projSpeed;
    public int nrOfBullets;
    public float inaccuracy;
    public float speedVariance;
    public float magReloadTime;
    public int magSize;
    public float degreesBetweenBullets;
    public float degreesBetweenShots;
    public float startingAngleDegrees;
    public float projLifeTime;
    public bool ignoreRotation;
  }
}
