using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Planet
{
  public class Weapon
  {
    protected Ship ship;
    protected World world;

    public float damage;
    public float shotsPerSecond;
    public int nrOfBullets;
    public float projSpeed;
    public float projLifeTime;
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
    protected internal float secondsToNextShot;
    protected internal float secondsToNextReload;
    protected internal int currentMagCount;
    protected internal float currentBulletAngle;
    protected internal float currentShotAngle;

    public Weapon(
        Ship ship,
        World world,
        float damage,
        float shotsPerSecond,
        float projSpeed,
        int nrOfBullets = 1,
        float inaccuracy = 0,
        float speedVariance = 0,
        float magReloadTime = 0,
        int magSize = 1,
        float degreesBetweenBullets = 0,
        float degreesBetweenShots = 0,
        float startingAngleDegrees = 0,
        float projLifeTime = 5)
    {
      this.ship = ship;
      this.world = world;

      this.damage = damage;
      this.shotsPerSecond = shotsPerSecond;
      this.projSpeed = projSpeed;
      this.nrOfBullets = nrOfBullets;
      this.inaccuracy = inaccuracy;
      this.speedVariance = speedVariance;
      this.magReloadTime = magReloadTime;
      this.magSize = magSize;
      this.degreesBetweenBullets = degreesBetweenBullets;
      this.degreesBetweenShots = degreesBetweenShots;
      this.startingAngleDegrees = startingAngleDegrees;
      this.projLifeTime = projLifeTime;

      this.currentMagCount = magSize;
    }

    public Weapon(Ship ship, World world, WpnDesc desc)
    {
      this.ship = ship;
      this.world = world;

      this.damage = desc.damage;
      this.shotsPerSecond = desc.shotsPerSecond;
      this.projSpeed = desc.projectileSpeed;
      this.nrOfBullets = desc.nrOfBullets;
      this.inaccuracy = desc.inaccuracy;
      this.speedVariance = desc.speedVariance;
      this.magReloadTime = desc.magReloadTime;
      this.magSize = desc.magSize;
      this.degreesBetweenBullets = desc.degreesBetweenBullets;
      this.degreesBetweenShots = desc.degreesBetweenShots;
      this.startingAngleDegrees = desc.startingAngleDegrees;
      this.projLifeTime = desc.projLifeTime;

      this.currentMagCount = magSize;
    }


    public Weapon(Ship ship, World world)
    {
      this.ship = ship;
      this.world = world;
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
        currentShotAngle += MathHelper.ToRadians(degreesBetweenShots);
        if (shotsPerSecond != 0)
          secondsToNextShot = 1 / shotsPerSecond;
        currentMagCount--;
      }
    }

    private void Reload()
    {
      secondsToNextReload = magReloadTime;
      currentMagCount = magSize;
      currentShotAngle = 0;
    }

    protected virtual void Shoot()
    {
      currentBulletAngle = MathHelper.ToRadians(startingAngleDegrees);
      for (int i = 0; i < nrOfBullets; i++)
      {
        CreateBullet();
        currentBulletAngle += MathHelper.ToRadians(degreesBetweenBullets);
      }
    }

    protected virtual void CreateBullet()
    {
      Vector2 direction = Utility.AngleToVector2(ship.Rotation + currentBulletAngle + currentShotAngle);
      if (inaccuracy != 0)
        ApplyInaccuracy(ref direction, inaccuracy);
      float sv = Utility.GetRandom(Game1.rnd, -speedVariance, speedVariance);

      Projectile p = new Projectile(
        world,
        AssetManager.GetTexture("Proj1"),
        ship.Pos,
        direction,
        sv + projSpeed,
        damage,
        ship,
        projLifeTime,
        BulletPattern
        );

      world.PostProjectile(p);
    }

    protected virtual void BulletPattern(Projectile p, GameTime gt)
    {
      p.Pos += p.velocity * (float)gt.ElapsedGameTime.TotalSeconds;
    }
    protected void ApplyInaccuracy(ref Vector2 dir, float inaccuracy)
    {
      float deviation = Utility.GetRandom(Game1.rnd, -inaccuracy, inaccuracy);
      dir = Utility.RotateVector2(dir, Vector2.Zero, MathHelper.ToRadians(deviation));
    }

    public WpnDesc GetDesc()
    {
      WpnDesc desc = new WpnDesc(
          this.damage,
          this.shotsPerSecond,
          this.projSpeed,
          this.nrOfBullets,
          this.inaccuracy,
          this.speedVariance,
          this.magReloadTime,
          this.magSize,
          this.degreesBetweenBullets,
          this.degreesBetweenShots,
          this.startingAngleDegrees,
          this.projLifeTime);
      return desc;
    }
    public void SetDesc(WpnDesc desc)
    {
      this.damage = desc.damage;
      this.shotsPerSecond = desc.shotsPerSecond;
      this.projSpeed = desc.projectileSpeed;
      this.nrOfBullets = desc.nrOfBullets;
      this.inaccuracy = desc.inaccuracy;
      this.speedVariance = desc.speedVariance;
      this.magReloadTime = desc.magReloadTime;
      this.magSize = desc.magSize;
      this.degreesBetweenBullets = desc.degreesBetweenBullets;
      this.degreesBetweenShots = desc.degreesBetweenShots;
      this.startingAngleDegrees = desc.startingAngleDegrees;
      this.projLifeTime = desc.projLifeTime;

      this.currentMagCount = magSize;
    }

    public WeaponState GetState()
    {
      return new WeaponState(this);
    }
    public void SetState(WeaponState ws)
    {
      this.secondsToNextShot = ws.secondsToNextShot;
      this.secondsToNextShot = ws.secondsToNextShot;
      this.secondsToNextReload = ws.secondsToNextReload;
      this.currentMagCount = ws.currentMagCount;
      this.currentBulletAngle = ws.currentBulletAngle;
      this.currentShotAngle = ws.currentShotAngle;
    }
  }

  public class WeaponState
  {
    public float secondsToNextShot;
    public float secondsToNextReload;
    public int currentMagCount;
    public float currentBulletAngle;
    public float currentShotAngle;

    public WeaponState(Weapon wpn)
    {
      secondsToNextShot = wpn.secondsToNextShot;
      secondsToNextReload = wpn.secondsToNextReload;
      currentMagCount = wpn.currentMagCount;
      currentBulletAngle = wpn.currentBulletAngle;
      currentShotAngle = wpn.currentShotAngle;
    }
  }
}
