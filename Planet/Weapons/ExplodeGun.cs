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
      desc.damage = 30;
      desc.nrOfBullets = 1;
      desc.shotsPerSecond = 1.75f;
      desc.projSpeed = 400;
      desc.projLifeTime = 0.7f;
      desc.magSize = 4;
      desc.magReloadTime = 1;
      SetDesc(desc);
    }
    public ExplodeGun(ExplodeGun other)
      : base(other)
    {
      SetDesc(other.desc);
    }
    void Explode(Projectile p)
    {
      int N = 24;
      float R = (float)Math.PI / 4;
      float speedVar = 150;
      float spread = 5;
      float angle = Utility.Vector2ToAngle(p.velocity) - R / 2;

      for (int i = 0; i < N; i++)
      {
        Vector2 direction = Utility.AngleToVector2(angle);
        ApplyInaccuracy(ref direction, spread);
        angle += R / (N - 1);
        float sv = Utility.RandomFloat(-speedVar, speedVar);
        Projectile p2 = new Projectile(
          world,
          ProjTex,
          p.Pos,
          direction,
          sv + desc.projSpeed,
          Damage / 5,
          ship,
          1.0f + Utility.RandomFloat(-0.1f, 0.1f),
          FragmentPattern);
        p2.Scale = Scale * 0.3f;
        world.PostProjectile(p2);
      }
    }
    protected override void BulletPattern(Projectile p, GameTime gt)
    {
      if (p.lifeTimer.Finished)
      {
        Explode(p);
      }
      else
      {
        p.Pos += p.velocity * (float)gt.ElapsedGameTime.TotalSeconds;
        p.Rotation += (float)(2 * Math.PI * gt.ElapsedGameTime.TotalSeconds);
      }
    }
    protected void FragmentPattern(Projectile p, GameTime gt)
    {
      if (p.lifeTimer.Remaining < 0.5)  //Fade-out effect
        p.alpha = (float)(0.25 + p.lifeTimer.Remaining / 0.5);
      p.Pos += p.velocity * (float)gt.ElapsedGameTime.TotalSeconds;
      p.Rotation += (float)(4 * Math.PI * gt.ElapsedGameTime.TotalSeconds);
    }
  }
}
