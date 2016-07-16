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
        : base(ship, world)
    {
      SetDesc(WpnDesc.Circle(24));
      projSpeed = 200;
      projLifeTime = 5;
      magSize = 5;
      magReloadTime = 3;
      inaccuracy = 5;
      speedVariance = 50;
    }

    public override void Update(GameTime gt)
    {

      base.Update(gt);
    }

    protected override void Shoot()
    {
      currentBulletAngle = MathHelper.ToRadians(startingAngleDegrees);
      Projectile center = new Projectile(world, null, ship.Pos, ship.GetDirection(), projSpeed, 0, null, projLifeTime, null);
      center.Scale = 1.0f;
      center.layerMask = Layer.ZERO;
      world.PostProjectile(center);

      for (int i = 0; i < nrOfBullets; i++)
      {
        CreateBullet(center);
        currentBulletAngle += MathHelper.ToRadians(degreesBetweenBullets);
      }
    }

    private void CreateBullet(Projectile center)
    {
      Vector2 direction = Utility.AngleToVector2(ship.Rotation + currentBulletAngle + currentShotAngle);
      if (inaccuracy != 0)
        ApplyInaccuracy(ref direction, inaccuracy);
      float sv = Utility.GetRandom(Game1.rnd, -speedVariance, speedVariance);

      Projectile p = new Projectile(
          world,
          AssetManager.GetTexture("Proj1"),
          ship.Pos,
          direction,
          sv + projSpeed,
          damage,
          ship,
          projLifeTime,
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
      else if (p.frame > 11 && p.frame < 90)
        p.velocity = Vector2.Transform(p.velocity, Matrix.CreateRotationZ(0.06f));
      else if (p.frame == 10)
        p.velocity = -dir2 * p.speed;
      else if (p.frame == 110)
      {
        p.Die();

        //test
        for (int i = -1; i <= 1; i += 2)
        {
          Vector2 direction = new Vector2(p.velocity.Y, p.velocity.X * i);
          ApplyInaccuracy(ref direction, 10);
          float sv = Utility.GetRandom(Game1.rnd, -50, 50);
          Projectile p2 = new Projectile(
            world,
            AssetManager.GetTexture("Proj1"),
            p.Pos,
            direction,
            sv + projSpeed,
            damage,
            ship,
            3,
            null);
          world.PostProjectile(p2);
        }
      }
      p.Pos += p.velocity * (float)gt.ElapsedGameTime.TotalSeconds;
    }
  }
}
