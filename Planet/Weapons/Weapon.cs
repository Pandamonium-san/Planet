using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Planet
{
  public class Weapon : Transform
  {
    public string Name { get; set; }
    public WpnDesc Desc { get { return desc; } }

    protected Texture2D projTex;
    public Ship ship;
    protected World world;
    protected WpnDesc desc;

    // counter variables
    protected int currentMagCount;
    protected float currentBulletAngle;
    protected float currentShotAngle;
    protected Timer shootTimer;

    public Weapon(Ship ship, World world, WpnDesc desc, string pTex = "proj1", string name = "Unnamed Weapon")
      : base(Vector2.Zero, 0, 1.0f, ship)
    {
      Name = name;
      if (pTex == "")
        projTex = null;
      else
        projTex = AssetManager.GetTexture(pTex);
      this.ship = ship;
      this.world = world;
      SetDesc(desc);
      SetMuzzle(Vector2.Zero);
    }
    public Weapon(Weapon wpn) : base(wpn.Pos, wpn.Rotation, wpn.Scale, wpn.Parent)
    {
      Name = wpn.Name;
      projTex = wpn.projTex;
      ship = wpn.ship;
      world = wpn.world;
      desc = wpn.Desc;
      currentMagCount = wpn.currentMagCount;
      currentBulletAngle = wpn.currentBulletAngle;
      currentShotAngle = wpn.currentShotAngle;
      shootTimer = wpn.shootTimer;
    }
    public virtual void Update(GameTime gt)
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
        //var sfx = AssetManager.GetSfx("Laser_Shoot").CreateInstance();
        //sfx.Volume = 0.3f;
        //if (ship is RewinderShip)
        //  sfx.Play();
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
        shotAngle += Rotation;
      Vector2 direction = Utility.AngleToVector2(shotAngle);
      if (desc.inaccuracy != 0)
        ApplyInaccuracy(ref direction, desc.inaccuracy);
      float sv = Utility.RandomFloat(-desc.speedVariance, desc.speedVariance);
      Projectile p = new Projectile(
        world,
        projTex,
        Pos,
        direction,
        sv + desc.projSpeed,
        desc.damage,
        ship,
        desc.projLifeTime,
        BulletPattern,
        OnProjectileCollision
        );
      p.Scale *= Scale;
      world.PostProjectile(p);
    }
    protected virtual void BulletPattern(Projectile p, GameTime gt)
    {
      p.Pos += p.velocity * (float)gt.ElapsedGameTime.TotalSeconds;
      p.Rotation = Utility.Vector2ToAngle(p.velocity);
    }
    protected virtual void OnProjectileCollision(Projectile p, GameObject other)
    {
      for (int i = 0; i < 3; i++)
      {
        world.Particles.CreateHitEffect(p.Pos, 0.3f, -100, 100, p.color, 0.5f, 0.5f, 0.3f);
      }

      //world.Particles.CreateParticle(p.Pos, AssetManager.GetTexture("laserBlue08"), Vector2.Zero, 0.2f, Color.White, 0.7f, 4f, p.Scale * 0.7f);
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
      LocalPos = pos;
      LocalRotation = rotation;
    }
    public void SetDesc(WpnDesc desc)
    {
      if (desc.magSize == 0)
        desc.magSize = 1;
      this.desc = desc;
      currentMagCount = desc.magSize;
      shootTimer = new Timer(100);
    }
    public void SetShip(Ship ship)
    {
      Vector2 localPos = LocalPos;
      float localRotation = LocalRotation;
      float localScale = LocalScale;
      this.Parent = ship;
      LocalPos = localPos;
      LocalRotation = localRotation;
      LocalScale = localScale;

      this.ship = ship;
    }
  }
}
