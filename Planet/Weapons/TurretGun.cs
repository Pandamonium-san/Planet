using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class TurretGun : Weapon
  {
    public TurretGun(Ship ship, World world)
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
        Vector2 direction = p.velocity;
        float near = 9999999;
        List<GameObject> gos = world.GetGameObjects();
        foreach (GameObject go in gos)
        {
          if (go.isActive && go.layer == Layer.ENEMY_SHIP)
          {
            float distance = Utility.DistanceSquared(p.Pos, go.Pos);
            if (distance < near)
            {
              near = distance;
              direction = (go.Pos) - p.Pos;
            }
          }
        }
        for (int i = -1; i < 2; i++)
        {
          Vector2 nDir = Utility.RotateVector2(direction, Vector2.Zero, i * 0.1f);
          Projectile p2 = new Projectile(
  world,
  AssetManager.GetTexture("Proj1"),
  p.Pos,
  nDir,
  desc.projSpeed * 3f,
  desc.damage,
  ship,
  5f,
  base.BulletPattern);
          world.PostProjectile(p2);
        }
      }
      if (p.frame == 500)
        p.Die();
      else if (p.frame < 120)
      {
        p.Pos += p.velocity * (float)gt.ElapsedGameTime.TotalSeconds;
        p.Rotation = Utility.Vector2ToAngle(p.velocity);
      }
    }
  }
}
