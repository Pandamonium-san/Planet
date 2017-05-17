using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class LaserGun : WHitScan
  {
    float prevRotation = 0f;
    Texture2D particle1, particle2;
    public LaserGun(Ship ship, World world, WpnDesc desc, int width, float range = 10000, bool red = false)
      : base(ship, world, desc, width, range)
    {
      if (red)
      {
        ProjTex = AssetManager.GetTexture("laserRed_m");
        particle1 = AssetManager.GetTexture("laserRed08");
        particle2 = AssetManager.GetTexture("laserRed10");
      }
      else
      {
        ProjTex = AssetManager.GetTexture("laserBlue_m");
        particle1 = AssetManager.GetTexture("laserBlue08");
        particle2 = AssetManager.GetTexture("laserBlue10");
      }
    }
    public LaserGun(LaserGun other) : base(other)
    {
      particle1 = other.particle1;
      particle2 = other.particle2;
      prevRotation = other.prevRotation;
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
      world.Particles.CreateParticle(Pos, particle1, prDir * speed, lifeTime, Color.White, 0.5f, 4f, scale);
    }
    protected override void OnProjectileCollision(Projectile p, GameObject other)
    {
      if (!(p is PHitScan))
        return;
      PHitScan ph = (PHitScan)p;
      Vector2 prPos = ph.hitPos;
      Particle pr = world.Particles.CreateParticle(prPos, particle2, Vector2.Zero, 0.1f, Color.White, 0.8f, 4f, 1.0f * width / 20f);
      pr.Rotation = prevRotation += (float)Math.PI / 24;
      if (other.frame % 2 == 0)
        return;

      Vector2 prDir = -ph.dir;
      ApplyInaccuracy(ref prDir, 65);
      float speed = Utility.RandomFloat(150, 200);
      float lifeTime = Utility.RandomFloat(0.5f, 0.8f);
      float scale = Utility.RandomFloat(0.1f, 0.3f) * width / 20f;
      world.Particles.CreateParticle(prPos, particle1, prDir * speed, lifeTime, Color.White, 0.5f, 4f, scale);
    }
  }
}
