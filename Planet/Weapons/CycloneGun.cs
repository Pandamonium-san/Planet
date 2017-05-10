using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  //test class
  class CycloneGun : Weapon
  {
    public CycloneGun(Ship ship, World world)
        : base(ship, world, new WpnDesc())
    {
      desc.damage = 10;
      desc.nrOfBullets = 24;
      desc.degreesBetweenBullets = 360 / desc.nrOfBullets;
      desc.shotsPerSecond = 3;
      desc.projSpeed = 200;
      desc.projLifeTime = 5;
      desc.magSize = 3;
      desc.magReloadTime = 1;
      SetDesc(desc);
    }
    public CycloneGun(CycloneGun other) : base(other)
    {
      SetDesc(other.desc);
    }
    protected override void Shoot()
    {
      currentBulletAngle = MathHelper.ToRadians(desc.startingAngleDegrees);
      Projectile center = new Projectile(
        world,
        null,
        ship.Pos,
        ship.Forward,
        desc.projSpeed,
        0,
        null,
        desc.projLifeTime,
        (p, gt) => { p.Pos += p.velocity * (float)gt.ElapsedGameTime.TotalSeconds; });
      center.Scale = 1.0f;
      center.LayerMask = Layer.ZERO;
      center.Visible = false;
      world.PostProjectile(center);

      for (int i = 0; i < desc.nrOfBullets; i++)
      {
        CreateBullet(center);
        currentBulletAngle += MathHelper.ToRadians(desc.degreesBetweenBullets);
      }
    }

    private void CreateBullet(Projectile center)
    {
      Vector2 direction = Utility.AngleToVector2(ship.Rotation + currentBulletAngle + currentShotAngle);
      if (desc.inaccuracy != 0)
        ApplyInaccuracy(ref direction, desc.inaccuracy);
      float sv = Utility.RandomFloat(-desc.speedVariance, desc.speedVariance);

      Projectile p = new Projectile(
          world,
          ProjTex,
          ship.Pos,
          direction,
          sv + desc.projSpeed,
          Damage,
          ship,
          desc.projLifeTime,
          BulletPattern);
      p.Parent = center;
      world.PostProjectile(p);
    }

    protected override void BulletPattern(Projectile p, GameTime gt)
    {
      Vector2 dir2 = new Vector2(p.dir.Y, -p.dir.X);

      if (p.frame == 90)
      {
        Vector2 dir = p.Parent.Pos - p.Pos;
        dir.Normalize();
        p.velocity = dir * p.speed;
        ((Projectile)(p.Parent)).velocity = Vector2.Zero;
      }
      else if (p.frame > 1 && p.frame < 90)
        p.velocity = Vector2.Transform(p.velocity, Matrix.CreateRotationZ(0.06f));
      else if (p.frame == 10)
        p.velocity = -dir2 * p.speed;
      else if (p.frame == 91)
      {
        p.Die();

        //test
        for (int i = -1; i <= 1; i += 2)
        {
          Vector2 direction = new Vector2(p.velocity.X, p.velocity.Y);
          ApplyInaccuracy(ref direction, 10);
          float sv = Utility.RandomFloat(-50, 50);
          Projectile p2 = new Projectile(
            world,
            ProjTex,
            p.Pos,
            direction,
            sv + desc.projSpeed*1.5f,
            Damage,
            ship,
            1f,
            base.BulletPattern);
          world.PostProjectile(p2);
        }
      }
      p.Pos += p.velocity * (float)gt.ElapsedGameTime.TotalSeconds;
      p.Rotation = Utility.Vector2ToAngle(p.velocity);
    }
  }
}
