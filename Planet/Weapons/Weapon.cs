using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Planet
{
  public class Weapon
  {
    public string Name { get; set; }
    public WpnDesc Desc { get { return desc; } }
    protected Ship ship;
    protected World world;
    protected Transform muzzle;
    protected WpnDesc desc;

    // counter variables
    protected int currentMagCount;
    protected float currentBulletAngle;
    protected float currentShotAngle;
    protected Timer shootTimer;

    public Weapon(Ship ship, World world, WpnDesc desc)
    {
      Name = "Unnamed Weapon";
      this.ship = ship;
      this.world = world;
      SetDesc(desc);
      if (ship is RewinderShip)
        Console.Write("hello");
      muzzle = new Transform(Vector2.Zero, 0, 0, ship);
      SetMuzzle(Vector2.Zero);
    }
    public void Update(GameTime gt)
    {
      shootTimer.Update(gt);
      if ((shootTimer.elapsedSeconds >= desc.magReloadTime && desc.magReloadTime > 0) ||
      (desc.magReloadTime == 0 && currentMagCount == 0))  // edge case where reload time is 0 and magazine is used to determine shot angle
      {
        Reload();
      }
    }
    public void Fire()
    {
      if (currentMagCount > 0 && CanShoot())
      {
        Shoot();
        currentShotAngle += MathHelper.ToRadians(desc.degreesBetweenShots);
        currentMagCount--;
        shootTimer.Start();
      }
    }
    public void ResetShootTimer()
    {
      shootTimer.Start();
    }
    private void Reload()
    {
      currentMagCount = desc.magSize;
      currentShotAngle = 0;
    }
    protected virtual void Shoot()
    {
      currentBulletAngle = MathHelper.ToRadians(desc.startingAngleDegrees);
      for (int i = 0; i < desc.nrOfBullets; i++)
      {
        CreateBullet();
        currentBulletAngle += MathHelper.ToRadians(desc.degreesBetweenBullets);
      }
    }
    protected virtual void CreateBullet()
    {
      float shotAngle = currentBulletAngle + currentShotAngle;
      if (!desc.ignoreRotation)
        shotAngle += muzzle.Rotation;
      Vector2 direction = Utility.AngleToVector2(shotAngle);
      if (desc.inaccuracy != 0)
        ApplyInaccuracy(ref direction, desc.inaccuracy);
      float sv = Utility.RandomFloat(-desc.speedVariance, desc.speedVariance);

      Projectile p = new Projectile(
        world,
        "Proj1",
        muzzle.Pos,
        direction,
        sv + desc.projSpeed,
        desc.damage,
        ship,
        desc.projLifeTime,
        BulletPattern
        );

      world.PostProjectile(p);
    }
    protected virtual void BulletPattern(Projectile p, GameTime gt)
    {
      p.Pos += p.velocity * (float)gt.ElapsedGameTime.TotalSeconds;
      p.Rotation = Utility.Vector2ToAngle(p.velocity);
    }
    protected void ApplyInaccuracy(ref Vector2 dir, float inaccuracy)
    {
      float deviation = Utility.RandomFloat(-inaccuracy, inaccuracy);
      dir = Utility.RotateVector2(dir, Vector2.Zero, MathHelper.ToRadians(deviation));
    }
    public bool CanShoot()
    {
      return shootTimer.elapsedSeconds >= 1.0f / desc.shotsPerSecond;
    }
    public void SetMuzzle(Vector2 pos, float rotation = 0)
    {
      muzzle.localPos = pos;
      muzzle.localRotation = rotation;
    }
    public void SetDesc(WpnDesc desc)
    {
      if (desc.magSize == 0)
        desc.magSize = 1;
      this.desc = desc;
      currentMagCount = desc.magSize;
      shootTimer = new Timer(100);
    }
  }
}
