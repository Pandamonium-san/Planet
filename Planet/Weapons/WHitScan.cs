﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class WHitScan : Weapon
  {
    protected int width;
    protected float range;
    protected bool canPierce;
    public WHitScan(Ship ship, World world, WpnDesc desc, int width, bool canPierce, float range = 10000)
      : base(ship, world, desc)
    {
      this.width = width;
      this.range = range;
      this.canPierce = canPierce;
      this.desc.projSpeed = 10000;
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
        projTex,
        Pos,
        direction,
        desc.damage,
        canPierce,
        width,
        range,
        ship);
      p.onCollision = OnProjectileCollision;
      world.PostProjectile(p);
    }
  }
}
