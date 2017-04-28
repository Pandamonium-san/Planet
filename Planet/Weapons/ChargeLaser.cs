using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Planet
{
  class ChargeLaser : LaserGun
  {
    // shots per second = charge rate
    // magazine size = max charge level
    float chargeLevel;
    bool holding;
    public ChargeLaser(Ship ship, World world, WpnDesc desc, int width, bool canPierce = true, float range = 10000)
      : base(ship, world, desc, width, canPierce, range)
    {
    }
    public ChargeLaser(ChargeLaser other)
      : base(other)
    {
      chargeLevel = other.chargeLevel;
      holding = other.holding;
    }
    public override void Update(GameTime gt)
    {
      base.Update(gt);
      if (chargeLevel > 0 && !holding)
      {
        Release();
      }
      holding = false;
    }
    public override void Fire()
    {
      holding = true;
      if (chargeLevel < desc.magSize)
      {
        Charge();
      }
    }
    void Charge()
    {
      chargeLevel += desc.shotsPerSecond;

      Vector2 prDir = ship.Forward;
      ApplyInaccuracy(ref prDir, 55);
      float speed = Utility.RandomFloat(150, 275);
      float lifeTime = Utility.RandomFloat(0.15f, 0.3f);
      float scale = Utility.RandomFloat(0.12f, 0.26f) * width / 20f;
      Vector2 pos = Pos + prDir * speed * lifeTime;
      Particle p = world.Particles.CreateParticle(pos, AssetManager.GetTexture("laserBlue08"), -prDir * speed, lifeTime, Color.White, 0.5f, 4f, scale);
      p.Parent = this;
    }
    void Release()
    {
      base.Shoot();
      chargeLevel -= 1;
      Reload();
    }
  }
}
