using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class ExplodeGun : Weapon
  {
    public ExplodeGun(Ship ship, World world) 
      : base(ship, world, new WpnDesc())
    {
      desc.damage = 10;
      desc.nrOfBullets = 1;
      desc.shotsPerSecond = 3;
      desc.projSpeed = 200;
      desc.projLifeTime = 5;
      desc.magSize = 3;
      desc.magReloadTime = 1;
      SetDesc(desc);
    }
    protected override void BulletPattern(Projectile p, GameTime gt)
    {
      if (p.frame >= 120 && p.frame % 10 == 0)
      {
        float angle = 0f;
        for (int i = 0; i < 64; i++)
        {
          Vector2 direction = Utility.AngleToVector2(angle);
          ApplyInaccuracy(ref direction, 10);
          angle += MathHelper.ToRadians(360 / 64f);
          float sv = Utility.RandomFloat(-150, 150);
          Projectile p2 = new Projectile(
            world,
            AssetManager.GetTexture("Proj1"),
            p.Pos,
            direction,
            sv + desc.projSpeed * 1.5f,
            desc.damage,
            ship,
            5f,
            base.BulletPattern);
          world.PostProjectile(p2);
        }
        if (p.frame == 200)
          p.Die();
      }
      else if (p.frame < 120)
      {
        p.Pos += p.velocity * (float)gt.ElapsedGameTime.TotalSeconds;
        p.Rotation = Utility.Vector2ToAngle(p.velocity);
      }
    }
  }
}
