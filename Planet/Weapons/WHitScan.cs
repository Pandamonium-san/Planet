using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class WHitScan : Weapon
  {
    private int width;
    private bool canPierce;
    public WHitScan(Ship ship, World world, WpnDesc desc, int width, bool canPierce)
      : base(ship, world, desc)
    {
      this.width = width;
      this.canPierce = canPierce;
    }
    protected override void CreateBullet()
    {
      float shotAngle = currentBulletAngle + currentShotAngle;
      if (!desc.ignoreRotation)
        shotAngle += muzzle.Rotation;
      Vector2 direction = Utility.AngleToVector2(shotAngle);
      if (desc.inaccuracy != 0)
        ApplyInaccuracy(ref direction, desc.inaccuracy);

      PHitScan p = new PHitScan(
        world,
        muzzle.Pos,
        direction,
        desc.damage,
        canPierce,
        width,
        ship);
      world.PostProjectile(p);
    }
    
  }
}
