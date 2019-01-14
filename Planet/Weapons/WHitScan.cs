using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class WHitScan : Weapon
  {
    public bool OriginSticks { get; set; }
    protected int width;
    protected float range;
    public WHitScan(Ship ship, World world, WpnDesc desc, int width, float range = 10000)
      : base(ship, world, desc)
    {
      OriginSticks = true;
      this.width = width;
      this.range = range;
      this.desc.projSpeed = 10000;
    }
    public WHitScan(WHitScan other) : base(other)
    {
      width = other.width;
      range = other.range;
      desc.projSpeed = 10000;
    }
    protected override void CreateBullet()
    {
      float shotAngle = currentBulletAngle + currentShotAngle;
      if (!desc.ignoreRotation)
        shotAngle += Rotation;
      Vector2 direction = Utility.AngleToVector2(shotAngle);
      if (desc.inaccuracy != 0)
        ApplyInaccuracy(ref direction, desc.inaccuracy);

      PHitScan p = new PHitScan(
        world,
        ProjTex,
        Pos,
        direction,
        Damage,
        desc.piercing,
        width,
        range,
        ship,
        this,
        desc.projLifeTime);
      p.stick = OriginSticks;
      p.onCollision = OnProjectileCollision;
      p.InvulnOnHit = InvulnOnHit;
      world.PostProjectile(p);
    }
  }
}
