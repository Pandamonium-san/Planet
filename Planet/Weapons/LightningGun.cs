using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class LightningGun : Weapon
  {
    int width;
    public LightningGun(Ship ship, World world, int width)
      : base(ship, world, new WpnDesc())
    {
      this.width = width;

      desc.damage = 10;
      desc.inaccuracy = 0;
      desc.nrOfBullets = 3;
      desc.shotsPerSecond = 2;
      desc.projLifeTime = 0.5f;
      SetDesc(desc);
    }
    protected override void CreateBullet()
    {
      float shotAngle = currentBulletAngle + currentShotAngle;
      if (!desc.ignoreRotation)
        shotAngle += muzzle.Rotation;
      Vector2 direction = Utility.AngleToVector2(shotAngle);
      if (desc.inaccuracy != 0)
        ApplyInaccuracy(ref direction, desc.inaccuracy);
      PLightning p = new PLightning(
        world,
        muzzle.Pos,
        direction,
        desc.damage,
        width,
        ship,
        desc.projLifeTime);
      world.PostProjectile(p);
    }
  }
}
