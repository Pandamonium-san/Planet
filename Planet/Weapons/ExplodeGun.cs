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
      desc.damage = 20;
      desc.nrOfBullets = 1;
      desc.shotsPerSecond = 3;
      desc.projSpeed = 400;
      desc.projLifeTime = 3;
      desc.magSize = 3;
      desc.magReloadTime = 1;
      SetDesc(desc);
    }
    void Explode(Projectile p)
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
          projTex,
          p.Pos,
          direction,
          sv + desc.projSpeed,
          desc.damage / 5,
          ship,
          0.2f + Utility.RandomFloat(-0.1f, 0.1f),
          base.BulletPattern);
        world.PostProjectile(p2);
      }
    }
    protected override void BulletPattern(Projectile p, GameTime gt)
    {
      if (p.frame >= 34)
      {
        Explode(p);
        if (p.frame >= 35)
          p.Die();
      }
      else if (p.frame < 120)
      {
        p.Pos += p.velocity * (float)gt.ElapsedGameTime.TotalSeconds;
        p.Rotation = Utility.Vector2ToAngle(p.velocity);
      }
    }
    protected override void OnProjectileCollision(Projectile p, GameObject other)
    {
      base.OnProjectileCollision(p, other);
      Explode(p);
    }
  }
}
