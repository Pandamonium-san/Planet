using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class BlinkerShip : Ship
  {
    Color blinkColor = Color.DeepSkyBlue;
    float blinkRange = 200;
    float blinkDelay = 0.5f;
    Timer blinkTimer1;
    Timer blinkTimer2;
    Timer blinkTimer3;
    Vector2 blinkDirection;
    public BlinkerShip(Vector2 pos, World world)
        : base(pos, world, AssetManager.GetTexture(@"ships\blue\spaceShips_001"))
    {
      flashTex = AssetManager.GetTexture(@"ships\flash\spaceShips_001");
      SetLayer(Layer.PLAYER_SHIP);

      maxHealth = 100;
      currentHealth = maxHealth;
      maxShield = 20;
      currentShield = maxShield;
      LeadShots = true;
      Hitbox.LocalScale = 0.5f;

      blinkTimer1 = new Timer(blinkDelay, Blink1, false);
      blinkTimer2 = new Timer(blinkDelay, Blink2, false);
      blinkTimer3 = new Timer(blinkDelay, Blink3, false);

      WpnDesc desc = new WpnDesc(15, 1, 700, 12, 10, 100, 0, 1, 0, 0, 0, 1);              // burst shotgun
      Weapon wpn = new Weapon(this, world, desc, "laserBlue07");
      wpn.Name = "Shotgun2";
      weapons.Add(wpn);

      desc = new WpnDesc(5, 60, 500, 8, 10, 50, 1, 30, 360 / 8f, 360 / 30f, 0, 0.2f);                 // spinny projectile thing
      wpn = new Weapon(this, world, desc, "laserBlue10");
      wpn.Scale = 0.5f;
      wpn.Name = "Spin";
      weapons.Add(wpn);
    }
    public override void Update(GameTime gt)
    {
      base.Update(gt);
      blinkTimer1.Update(gt);
      blinkTimer2.Update(gt);
      blinkTimer3.Update(gt);
    }
    public override void Fire1()
    {
      CurrentWeapon.Fire();
    }
    // ship disappears, particles spawn
    public override void Fire2()
    {
      if (blinkTimer2.Counting)
        return;
      Flash(0.15f, blinkColor, false, 1.0f, true);
      blinkDirection = movementDirection;
      IsActive = false;
      Visible = false;
      CollisionEnabled = false;
      CreateBlinkParticle(false);
      CreateBlinkParticle2();
      blinkTimer1.Start();
    }
    // particles reappear
    private void Blink1()
    {
      Vector2 dir = blinkDirection;
      if (dir != Vector2.Zero)
        dir.Normalize();
      Pos += dir * blinkRange;

      CreateBlinkParticle(true);
      blinkTimer2.Start();
    }
    // ship reappears
    private void Blink2()
    {
      Flash(0.5f, blinkColor, false, 0.9f);
      IsActive = true;
      Visible = true;
      CreateBlinkParticle2();
      blinkTimer3.Start();
    }
    // collision is enabled
    private void Blink3()
    {
      CollisionEnabled = true;
    }
    private void CreateBlinkParticle(bool implode)
    {
      int n = 6;
      Texture2D tex = AssetManager.GetTexture("star1");
      float speed = 400;
      float lifeTime = (float)blinkTimer1.seconds;
      float alpha = 1f;
      float rotationSpeed = (float)Math.PI * 6;
      float scale = 1.3f;

      for (int i = 0; i < n; i++)
      {
        float angle = (float)(Math.PI * 2 / n * i);
        Vector2 pos = Pos;
        Vector2 dir = Utility.AngleToVector2(angle);
        if (implode)
        {
          pos += dir * speed * lifeTime;
          dir = -dir;
        }

        Projectile p = new Projectile(world, tex, pos, dir, speed, 40, this, lifeTime);
        p.Scale = scale;
        p.color = blinkColor;//Color.Transparent;
        p.Visible = false;
        world.PostProjectile(p);

        Particle pr = new Particle(pos, tex, dir * speed, lifeTime, blinkColor, alpha, rotationSpeed, scale);
        pr.FadeIn = implode;
        world.Particles.AddParticle(pr);
      }
    }
    private void CreateBlinkParticle2()
    {
      for (int i = 0; i < 15; i++)
        world.Particles.CreateStar(Pos, .7f, -50, 50, blinkColor, 0.7f, .5f, 0.4f);
    }
  }
}
