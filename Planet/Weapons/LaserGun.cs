using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class LaserGun : WHitScan
  {
    float prevRotation = 0f;
    public LaserGun(Ship ship, World world, WpnDesc desc, int width, bool canPierce, float range = 10000)
      : base(ship, world, desc, width, canPierce, range)
    {
      projTex = AssetManager.GetTexture("laserBlue_m");
    }
    protected override void Shoot()
    {
      base.Shoot();
      if (ship.frame % 4 != 0)
        return;
      Vector2 prDir = ship.Forward;
      ApplyInaccuracy(ref prDir, 55);
      float speed = Utility.RandomFloat(130, 175);
      float lifeTime = Utility.RandomFloat(0.4f, 0.6f);
      float scale = Utility.RandomFloat(0.08f, 0.20f) * width / 20f;
      world.Particles.CreateParticle(Pos, AssetManager.GetTexture("laserBlue08"), prDir * speed, lifeTime, Color.White, 0.5f, 4f, scale);
    }
    protected override void OnProjectileCollision(Projectile p, GameObject other)
    {
      if (!(p is PHitScan) || other.frame % 2 == 0)
        return;
      PHitScan ph = (PHitScan)p;
      Vector2 prPos = ph.hit;
      Particle pr = world.Particles.CreateParticle(prPos, AssetManager.GetTexture("laserBlue10"), Vector2.Zero, 0.1f, Color.White, 0.8f, 4f, 1.0f * width / 20f);
      pr.Rotation = prevRotation += (float)Math.PI/24;

      Vector2 prDir = -ph.dir;
      ApplyInaccuracy(ref prDir, 65);
      float speed = Utility.RandomFloat(150, 200);
      float lifeTime = Utility.RandomFloat(0.5f, 0.8f);
      float scale = Utility.RandomFloat(0.1f, 0.3f) * width / 20f;
      world.Particles.CreateParticle(prPos, AssetManager.GetTexture("laserBlue08"), prDir * speed, lifeTime, Color.White, 0.5f, 4f, scale);
    }
  }
}
