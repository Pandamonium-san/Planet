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
    public float Damage { get { return desc.damage * ship.damageModifier; } }
    public WpnDesc Desc { get { return desc; } }
    public Texture2D ProjTex { get; set; }
    public float InvulnOnHit { get; set; }
    public bool DashUsable { get; set; }
    public float ProjRotSpeed { get; set; }
    public SoundEffect SFX { get; set; }
    public float Volume { get; set; }
    public int ShotsPerSFX { get; set; }
    private int sfxShotsCounter;

    protected Ship ship;
    protected World world;
    protected WpnDesc desc;

    // counter variables
    protected int currentMagCount;
    protected float currentBulletAngle;
    protected float currentShotAngle;
    protected Timer shootTimer;

    public Weapon(Ship ship, World world, WpnDesc desc, string pTex = "proj1", string name = "Unnamed Weapon", string sfx = "pew", float volume = 0.16f)
      : base(Vector2.Zero, 0, 1.0f, ship)
    {
      Name = name;
      if (pTex == "")
        ProjTex = null;
      else
        ProjTex = AssetManager.GetTexture(pTex);
      ProjRotSpeed = 0;
      this.ship = ship;
      this.world = world;
      SetDesc(desc);
      SetMuzzle(Vector2.Zero);
      SFX = AssetManager.GetSfx(sfx);
      Volume = volume;
    }
    public Weapon(Weapon wpn)
      : base(wpn)
    {
      Name = wpn.Name;
      ProjTex = wpn.ProjTex;
      ship = wpn.ship;
      world = wpn.world;
      desc = wpn.Desc;
      currentMagCount = wpn.currentMagCount;
      currentBulletAngle = wpn.currentBulletAngle;
      currentShotAngle = wpn.currentShotAngle;
      shootTimer = new Timer(wpn.shootTimer);
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
    public virtual void Fire()
    {
      if (currentMagCount > 0 && CanShoot())
      {
        if (SFX != null && sfxShotsCounter-- <= 0)
        {
          sfxShotsCounter = ShotsPerSFX;
          SoundEffectInstance sei = SFX.CreateInstance();
          sei.Volume = Volume;
          sei.Play();
        }
        Shoot();
        currentShotAngle += MathHelper.ToRadians(desc.degreesBetweenShots);
        currentMagCount--;
        shootTimer.Start();
      }
    }
    public virtual void ResetShootTimer()
    {
      shootTimer.Start();
    }
    public virtual void FinishShootTimer()
    {
      shootTimer.ForceFinish();
    }
    public virtual void Reload()
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
        ProjTex,
        Pos,
        direction,
        sv + desc.projSpeed,
        Damage,
        ship,
        desc.projLifeTime,
        BulletPattern,
        OnProjectileCollision,
        desc.piercing
        );
      p.InvulnOnHit = InvulnOnHit;
      p.Scale *= Scale;
      world.PostProjectile(p);
    }
    protected virtual void BulletPattern(Projectile p, GameTime gt)
    {
      if (p.lifeTimer.Remaining < 0.5)  //Fade-out effect
        p.alpha = (float)(0.25 + p.lifeTimer.Remaining / 0.5);
      p.Pos += p.velocity * (float)gt.ElapsedGameTime.TotalSeconds;
      if (ProjRotSpeed > 0)
        p.Rotation += ProjRotSpeed * (float)gt.ElapsedGameTime.TotalSeconds;
      else
        p.Rotation = Utility.Vector2ToAngle(p.velocity);
    }
    protected virtual void OnProjectileCollision(Projectile p, GameObject other)
    {
      for (int i = 0; i < 3; i++)
      {
        world.Particles.CreateStar(p.Pos, 0.3f, -100, 100, p.color, 0.5f, 0.5f, 0.3f);
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
    public virtual void SetShip(Ship ship)
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
