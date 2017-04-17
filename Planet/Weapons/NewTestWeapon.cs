using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class NewTestWeapon : Weapon
  {
    readonly int nrOfProjectiles = 16;
    readonly float timeBetweenSpawns = 0.05f;

    GameObject hit;
    Timer timer;
    Vector2 impactPos;
    Texture2D projTex2;
    int spawned;
    public NewTestWeapon(Ship ship, World world, WpnDesc desc, string name) : base(ship, world, desc, "laserBlue16", name)
    {
      projTex2 = AssetManager.GetTexture("laserBlue07");
      timer = new Timer();
      spawned = 0;
    }
    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      timer.Update(gameTime);
    }
    protected override void BulletPattern(Projectile p, GameTime gt)
    {
      p.Pos += p.velocity * (float)gt.ElapsedGameTime.TotalSeconds;
      p.Rotation = Utility.Vector2ToAngle(p.velocity);
      if (p.frame % 5 == 0)
      {
        world.Particles.CreateParticle(p.Pos, AssetManager.GetTexture("laserBlue08"), -100, 100, -100, 100, 0.4f, Color.White, 0.7f, 4f, 0.1f);
      }
    }
    private void BulletPattern2(Projectile p, GameTime gt)
    {
      if (spawned < nrOfProjectiles)
      {
        p.Pos = hit.Pos - Vector2.Normalize(p.velocity) * 100;
        return;
      }
      p.CollisionEnabled = true;
      p.Pos += p.velocity * (float)gt.ElapsedGameTime.TotalSeconds;
      p.Rotation = Utility.Vector2ToAngle(p.velocity);
    }
    private void BulletRing()
    {
      float dirRnd = 1;// Utility.RandomFloat(0.80f, 1.20f);
      Vector2 direction = Utility.AngleToVector2(dirRnd * (spawned * (float)(2 * Math.PI) / nrOfProjectiles));
      float posRnd = 1;// Utility.RandomFloat(.3f, 1.8f);
      Vector2 position = impactPos - direction * 100 * posRnd;
      float lifeTime = 0.12f * posRnd;
      lifeTime += timeBetweenSpawns * (nrOfProjectiles - spawned);
      Projectile np = new Projectile(
        world,
        projTex2,
        position,
        direction,
        800,
        2,
        ship,
        lifeTime,
        BulletPattern2,
        base.OnProjectileCollision
        );
      np.Rotation = Utility.Vector2ToAngle(np.velocity);
      np.Pos = hit.Pos - Vector2.Normalize(np.velocity) * 100;
      np.CollisionEnabled = false;
      world.PostProjectile(np);
      if (++spawned < nrOfProjectiles)
        timer = new Timer(timeBetweenSpawns, BulletRing, true);
    }
    protected override void OnProjectileCollision(Projectile p, GameObject other)
    {
      spawned = 0;
      timer = new Timer(timeBetweenSpawns, BulletRing, true);
      impactPos = p.Pos;
      hit = other;
      world.Particles.CreateParticle(p.Pos, AssetManager.GetTexture("laserBlue08"), Vector2.Zero, 0.2f, Color.White, 0.5f, 0, 1.0f);
      for (int i = 0; i < 4; i++)
        world.Particles.CreateParticle(p.Pos, AssetManager.GetTexture("laserBlue08"), -100, 100, -100, 100, 0.4f, Color.White, 0.7f, 4f, 0.3f);
    }
  }
}
